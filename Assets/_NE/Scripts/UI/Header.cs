using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NextEdgeGames {
    public class Header : MonoBehaviour {

        private List<CurrencyButton> currencyButtons;

        [Header("Inspector Assigned")]
        [SerializeField] private Transform currencyButtonsContainer;
        [SerializeField] private Button button_Store;

        private void Awake() {
            currencyButtons = currencyButtonsContainer.GetComponentsInChildren<CurrencyButton>(true).ToList();
        }
        private void Start() {
            button_Store.onClick.AddListener(OnClickStoreButton);
            VirtualCurrency.instance.Events.onValueChange.AddListener(OnCurrencyUpdate);
            foreach (CurrencyButton cb in currencyButtons) {
                cb.UpdateCurrency();
            }
        }
        private void OnClickStoreButton() {
            UIManager.instance.OpenMenu(UIManager.MenuEnum.Store);
        }
        private void OnCurrencyUpdate(VirtualCurrency.CurrencyItem item) {
            CurrencyButton cb = currencyButtons.Find(x => x.Type == item.type);
            if (cb) {
                cb.UpdateCurrency(item);
            }
        }
    }
}