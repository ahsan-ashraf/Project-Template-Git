using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace ConnectGamesStudio {
    public class VirtualCurrency : MonoBehaviour {

        #region Serializable Nested Classes
        [Serializable] public class Event {
            [Serializable] public class CurrencyItem {
                public CurrencyType type;
                public int value;

                public CurrencyItem(CurrencyType type, int value) {
                    this.type = type;
                    this.value = value;
                }
            }

            public UnityEvent<CurrencyItem> onValueChange;
        }
        #endregion

        #region Public Static Fields
        public static VirtualCurrency instance;
        #endregion

        #region Serialized Private Fields
        [Header("Inspector Assigned")]
        [SerializeField] private List<string> currencyEnums;
        [SerializeField] private Event events;

        [Header("Runtime Information")]
        [SerializeField] private CurrencyType currencyTypes;
        [SerializeField] private List<Event.CurrencyItem> currencyItems;
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
        private void OnCurrencyUpdate(Event.CurrencyItem currencyItem) {
            CheckCurrencyItems();
        }
        private bool EnumListIsValid(List<string> list) {
            bool result = false;
            if (list.Count > 0) {
                result = true;
                foreach (string str in list) {
                    if (list.FindAll(x => x == str).Count > 1) {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }
        [EditorCools.Button(null, null, 5f)]
        private void GenerateEnum() {
            if (EnumListIsValid(this.currencyEnums)) {
                string enumName = "CurrencyType";
                string filePathAndName = "Assets/_NE/Scripts/Virtual Currency/" + enumName + ".cs"; //The folders _NE/Scripts/Virtual Currency/ is expected to exist

                using (StreamWriter streamWriter = new StreamWriter(filePathAndName)) {
                    streamWriter.WriteLine("namespace ConnectGamesStudio {");
                    streamWriter.WriteLine("\tpublic enum " + enumName + " {");
                    for (int i = 0; i < currencyEnums.Count; i++) {
                        streamWriter.WriteLine("\t\t" + currencyEnums[i] + (i != currencyEnums.Count - 1 ? "," : ""));
                    }
                    streamWriter.WriteLine("\t}");
                    streamWriter.WriteLine("}");
                }
                AssetDatabase.Refresh();
                Debug.Log(enumName + " enum is Generated Successfully!");
            } else {
                Debug.LogError("Invalid Enums list, Make sure list size is greater than zero and there are no repeating entries in list");
            }
        }
        [EditorCools.Button]
        private void CheckCurrencyItems() {
            currencyItems.Clear();
            foreach (string i in Enum.GetNames(typeof(CurrencyType))) {
                CurrencyType type = (CurrencyType)Enum.Parse(typeof(CurrencyType), i);
                int value = GetCurrency(type);
                currencyItems.Add(new Event.CurrencyItem(type, value));
                
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
            int currentAmount = GetCurrency(type);
            int total = currentAmount + amountToAdd;
            PlayerPrefs.SetInt(type.ToString(), total);
            if (amountToAdd > 0) {
                Events.onValueChange.Invoke(new Event.CurrencyItem(type, total));
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
            int currentAmount = GetCurrency(type);
            int remainingAmount = currentAmount - amountToSpend;
            if (remainingAmount >= 0) {
                PlayerPrefs.SetInt(type.ToString(), remainingAmount);
                result = true;
            }
            if (result) {
                Events.onValueChange.Invoke(new Event.CurrencyItem(type, remainingAmount));
            }
            return result;
        }

        /// <summary>
        /// Takes type of currency you want to inquire about.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Amount of currency stored against the provided type.</returns>
        public int GetCurrency(CurrencyType type) {
            return PlayerPrefs.GetInt(type.ToString(), 0);
        }
        #endregion

    }
}