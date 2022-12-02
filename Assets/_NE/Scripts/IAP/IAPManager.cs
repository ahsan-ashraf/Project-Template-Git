using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Collections.Generic;

namespace NextEdgeGames {
    public class IAPManager : MonoBehaviour, IStoreListener {

        private IStoreController controller;
        private IExtensionProvider extensionProvider;

        [Header("Inspector Assigned")]
        [SerializeField] private List<string> products;

        [Header("Runtime Information")]
        public IAPProducts availableProducts;

#if UNITY_EDITOR
        #region Private Methods
        private bool ProductListIsValid(List<string> list) {
            bool result = false;
            if (list != null && list.Count > 0) {
                result = true;
                foreach (string str in list) {
                    if (str.Length <= 0 || list.FindAll(x => x == str).Count > 1) {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }
        [EditorCools.Button(null, null, 5f)]
        private void GenerateProducts() {
            if (ProductListIsValid(products)) {
                string enumName = "IAPProducts";
                string filePathAndName = "Assets/_NE/Scripts/IAP/" + enumName + ".cs"; //The folders _NE/Scripts/Virtual Currency/ is expected to exist

                using (StreamWriter streamWriter = new StreamWriter(filePathAndName)) {
                    streamWriter.WriteLine("namespace NextEdgeGames {");
                    streamWriter.WriteLine("\tpublic enum " + enumName + " {");
                    for (int i = 0; i < products.Count; i++) {
                        string[] arr = products[i].Split('.');
                        string p = arr[arr.Length - 1];
                        streamWriter.WriteLine("\t\t" + p + (i != products.Count - 1 ? "," : ""));
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

        #endregion
#endif

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
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent) {
            Debug.Log(string.Format("IAP - Product {0} Purchased Successfully!", purchaseEvent.purchasedProduct.definition.id));
            return PurchaseProcessingResult.Complete;
        }
        #endregion
    }
}