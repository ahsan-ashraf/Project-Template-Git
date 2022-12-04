using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NextEdgeGames {
    public class Inventory : Menu {

        [Header("Inspector Assigned")]
        [SerializeField] private TextMeshProUGUI text_Title;
        [SerializeField] private Button button_Next;
        [SerializeField] private Button button_Buy;
        [SerializeField] private Button button_Back;
        [SerializeField] private UIManager.MenuEnum nextMenu;

        [SerializeField] private Camera garageCamera;

        public override UIManager.MenuEnum Type => UIManager.MenuEnum.Inventory;

        private void Start() {
            button_Next.onClick.AddListener(OnClickNextButton);
            button_Buy.onClick.AddListener(OnClickBuyButton);
            button_Back.onClick.AddListener(OnClickBackButton);
        }
        private void OnClickNextButton() {
            if (nextMenu != UIManager.MenuEnum.None)
                UIManager.instance.OpenMenu(nextMenu);
        }
        private void OnClickBuyButton() {}
        private void OnClickBackButton() {
            UIManager.instance.Back();
        }

        public override void SetActive(bool setActive) {
            gameObject.SetActive(setActive);
            text_Title.gameObject.SetActive(setActive);
            button_Back.gameObject.SetActive(setActive);
            garageCamera.gameObject.SetActive(setActive);
        }
    }
}