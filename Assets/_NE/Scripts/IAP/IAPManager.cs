using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;

namespace NextEdgeGames {
    public class IAPManager : MonoBehaviour, IStoreListener {

        [System.Serializable] private class ProductInfo {
            public string id;
            public ProductType type;
        }
        [System.Serializable] private class FinalProduct {
            public string id;
            public IAPProductID productEnum;

            public FinalProduct(string id, IAPProductID productEnum) {
                this.id = id;
                this.productEnum = productEnum;
            }
        }

        public static IAPManager instance;

        private IStoreController controller;
        private IExtensionProvider extensionProvider;

        private string productToPurchase;
        private UnityAction onSuccess;
        private UnityAction onFailure;
        private List<FinalProduct> finalProducts;

        [Header("Inspector Assigned")]
        [SerializeField] private List<ProductInfo> productInfo;

        [Header("Runtime Information")]
        public IAPProductID availableProducts;

        private void Awake() {
            if (instance == null) {
                instance = this;
            }
            else {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
        private IEnumerator Start() {
            yield return new WaitForSeconds(3f);
            Initialize();
        }
        private void OnValidate() {
            gameObject.name = "IAP Manager";
            gameObject.transform.position = Vector3.zero;
            gameObject.transform.rotation = Quaternion.identity;
        }
#if UNITY_EDITOR
        #region Private Methods
        private bool ProductListIsValid(List<ProductInfo> list) {
            bool result = false;
            if (list != null && list.Count > 0) {
                result = true;
                foreach (ProductInfo pInfo in list) {
                    if (pInfo.id.Length <= 0 || list.FindAll(x => x.id == pInfo.id).Count > 1) {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }
        [EditorCools.Button(null, null, 5f)]
        private void GenerateProducts() {
            if (ProductListIsValid(productInfo)) {
                string enumName = "IAPProductID";
                string filePathAndName = "Assets/_NE/Scripts/IAP/" + enumName + ".cs"; //The folders _NE/Scripts/Virtual Currency/ is expected to exist

                using (StreamWriter streamWriter = new StreamWriter(filePathAndName)) {
                    streamWriter.WriteLine("namespace NextEdgeGames {");
                    streamWriter.WriteLine("\tpublic enum " + enumName + " {");
                    for (int i = 0; i < productInfo.Count; i++) {
                        string[] arr = productInfo[i].id.Split('.');
                        string p = arr[arr.Length - 1];
                        streamWriter.WriteLine("\t\t" + p + (i != productInfo.Count - 1 ? "," : ""));
                    }
                    streamWriter.WriteLine("\t}");
                    streamWriter.WriteLine("}");
                }
                AssetDatabase.Refresh();
                Debug.Log(enumName + " enum is Generated Successfully!");
            } else {
                Debug.LogError("Invalid Enums list, Make sure list size is greater than zero and there are no repeating or empty entries in list");
            }
        }
        [EditorCools.Button("CheckPurchasedProduct(Works on Runtime Only)", null, 5f)]
        private void CheckPurchasedProduct() {
            foreach (FinalProduct item in finalProducts) {
                print(item.id + ": " + ((PlayerPrefs.GetInt(item.id, 0) == 1) ? "True" : "false"));
            }
        }
        #endregion
#endif

        public void Initialize() {
            finalProducts = new List<FinalProduct>();
            ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (ProductInfo pInfo in productInfo) {
                configurationBuilder.AddProduct(pInfo.id, pInfo.type);

                string[] arr = pInfo.id.Split('.');
                string p = arr[arr.Length - 1];
                IAPProductID productEnum = (IAPProductID)System.Enum.Parse(typeof(IAPProductID), p);
                finalProducts.Add(new FinalProduct(pInfo.id, productEnum));
            }
            UnityPurchasing.Initialize(this, configurationBuilder);
        }

        public bool isPurchased(IAPProductID productID) {
            bool result = false;
            FinalProduct fp = finalProducts.Find(x => x.productEnum == productID);
            if (fp != null) {
                result = PlayerPrefs.GetInt(fp.id, 0) == 1;
            }
            else {
                Debug.LogError("Invalid product id passed");
            }

            return result;
        }

        public void PurchaseProduct(IAPProductID productID, UnityAction onSuccess, UnityAction onFailure) {
            FinalProduct fp = finalProducts.Find(x => x.productEnum == productID);
            if (fp != null) {
                this.onSuccess = onSuccess;
                this.onFailure = onFailure;
                this.productToPurchase = fp.id;
                controller.InitiatePurchase(fp.id);
            }
            else {
                Debug.LogError("Invalid product id passed to purchase");
            }
        }

        #region IStoreListener Callbacks
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
            this.controller = controller;
            this.extensionProvider = extensions;
        }

        public void OnInitializeFailed(InitializationFailureReason error) {
            
            Debug.LogError(string.Format("IAP - Initialization Failed with error: {0}", error.ToString()));
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
            Debug.LogError(string.Format("IAP Purchase of product {0} Failed with error {1}", product.definition.id, failureReason.ToString()));
            if (product.definition.id == productToPurchase) {
                if (onFailure != null) {
                    onFailure();
                }
            }
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent) {
            Debug.Log(string.Format("IAP - Product {0} Purchased Successfully!", purchaseEvent.purchasedProduct.definition.id));
            if (purchaseEvent.purchasedProduct.definition.id == productToPurchase) {
                PlayerPrefs.SetInt(purchaseEvent.purchasedProduct.definition.id, 1);
                if (onSuccess != null) {
                    onSuccess();
                }
            }
            return PurchaseProcessingResult.Complete;
        }
        #endregion
    }
}