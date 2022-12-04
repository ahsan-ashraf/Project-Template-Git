using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace NextEdgeGames {
    public class VirtualCurrency : MonoBehaviour {

        #region Serializable Nested Classes
        [Serializable] private class CurrencyInfo {
            public string name;
            public Sprite icon;
            public int defaultAmount;
        }
        [Serializable] public class CurrencyItem {
            public CurrencyType type;
            public Sprite icon;
            public int value;

            public CurrencyItem(CurrencyType type, int value, Sprite icon) {
                this.type = type;
                this.value = value;
                this.icon = icon;
            }
        }
        [Serializable] public class Event {
            public UnityEvent<CurrencyItem> onValueChange;
        }
        #endregion

        #region Public Static Fields
        public static VirtualCurrency instance;
        #endregion

        #region Serialized Private Fields
        [Header("Inspector Assigned")]
        [SerializeField] private List<CurrencyInfo> info;
        [SerializeField] private Event events;

        [Header("Runtime Information")]
        [SerializeField] private CurrencyType currencyTypes;
        [SerializeField] private List<CurrencyItem> currencyItems;
        #endregion

        #region Properties
        public Event Events => events;
        #endregion

        #region Monobehaviour Callbacks
        private void Awake() {
            #region Singleton
            if (instance == null) {
                instance = this;
            }
            else {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
            #endregion
        }
        private void Start() {
#if UNITY_EDITOR
            Events.onValueChange.AddListener(OnCurrencyUpdate);
#endif
        }
        private void OnValidate() {
            gameObject.name = "Virtual Currency";
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
        #endregion

        #region Private Methods
#if UNITY_EDITOR
        private void OnCurrencyUpdate(CurrencyItem currencyItem) {
            CheckCurrencyItems();
        }
        private bool CurrencyListIsValid(List<CurrencyInfo> list) {
            bool result = false;
            if (list != null && list.Count > 0) {
                result = true;
                foreach (CurrencyInfo item in list) {
                    if (item.name.Length <= 0 || list.FindAll(x => x.name == item.name).Count > 1) {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }
        [EditorCools.Button(null, null, 5f)]
        private void GenerateCurrency() {
            if (CurrencyListIsValid(this.info)) {
                string enumName = "CurrencyType";
                string filePathAndName = "Assets/_NE/Scripts/Virtual Currency/" + enumName + ".cs"; //The folders _NE/Scripts/Virtual Currency/ is expected to exist

                using (StreamWriter streamWriter = new StreamWriter(filePathAndName)) {
                    streamWriter.WriteLine("namespace NextEdgeGames {");
                    streamWriter.WriteLine("\tpublic enum " + enumName + " {");
                    for (int i = 0; i < info.Count; i++) {
                        streamWriter.WriteLine("\t\t" + info[i].name + (i != info.Count - 1 ? "," : ""));
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
        [EditorCools.Button]
        private void CheckCurrencyItems() {
            currencyItems.Clear();
            foreach (string i in Enum.GetNames(typeof(CurrencyType))) {
                CurrencyType type = (CurrencyType)Enum.Parse(typeof(CurrencyType), i);
                CurrencyItem item = GetCurrency(type);
                currencyItems.Add(new CurrencyItem(type, item.value, item.icon));
                
            }
        }
#endif
        #endregion

        #region Public Methods
        /// <summary>
        /// Takes type of currency and amount to add against that type of cyrrency.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="amount"></param>
        public void AddCurrency(CurrencyType type, int amount) {
            int amountToAdd = Mathf.Abs(amount);
            int currentAmount = GetCurrency(type).value;
            int total = currentAmount + amountToAdd;
            PlayerPrefs.SetInt(type.ToString(), total);
            if (amountToAdd > 0) {
                CurrencyInfo inf = info.Find(X => X.name == type.ToString());
                Events.onValueChange.Invoke(new CurrencyItem(type, total, inf.icon));
            }
        }

        /// <summary>
        /// Takes type of currency and amount to spend(subbtract) against that type of cyrrency.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="amount"></param>
        /// <returns>True if amount spending is successfull, false otherwise.</returns>
        public bool SpendCurrency(CurrencyType type, int amount) {
            int amountToSpend = Mathf.Abs(amount);
            bool result = false;
            int currentAmount = GetCurrency(type).value;
            int remainingAmount = currentAmount - amountToSpend;
            if (remainingAmount >= 0) {
                PlayerPrefs.SetInt(type.ToString(), remainingAmount);
                result = true;
            }
            if (result) {
                CurrencyInfo inf = info.Find(X => X.name == type.ToString());
                Events.onValueChange.Invoke(new CurrencyItem(type, remainingAmount, inf.icon));
            }
            return result;
        }

        /// <summary>
        /// Takes type of currency you want to inquire about.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Amount of currency stored against the provided type.</returns>
        public CurrencyItem GetCurrency(CurrencyType type) {
            CurrencyInfo inf = info.Find(X => X.name == type.ToString());
            CurrencyItem ci = new CurrencyItem(type, PlayerPrefs.GetInt(type.ToString(), inf.defaultAmount), inf.icon);
            return ci;
        }
        #endregion

    }
}