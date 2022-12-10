using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NextEdgeGames {
    public class CurrencyButton : MonoBehaviour {

        private Button button;

        [SerializeField] private CurrencyType type;
        [SerializeField] private TextMeshProUGUI text_Value;
        [SerializeField] private Image image_Icon;
        [SerializeField] private bool autoUpdateIcon;

        public CurrencyType Type { get => type; }

        private void Awake() {
            button = GetComponent<Button>();
        }
        private void Start() {
            VirtualCurrency.CurrencyItem c = VirtualCurrency.instance.GetCurrency(Type);
            if (c != null) {
                text_Value.text = c.value.ToString();
                if (autoUpdateIcon) {
                    image_Icon.sprite = c.icon;
                }
            }
            button.onClick.AddListener(OnClickCurrencyButton);
        }
        private void OnClickCurrencyButton() {
            UIManager.instance.OpenMenu(UIManager.MenuEnum.Store);
        }

        public void UpdateCurrency(VirtualCurrency.CurrencyItem item = null) {
            VirtualCurrency.CurrencyItem ci = item != null ? item : VirtualCurrency.instance.GetCurrency(Type);
            text_Value.text = ci.value.ToString();
            image_Icon.sprite = autoUpdateIcon ? ci.icon : image_Icon.sprite;
        }
    }
}