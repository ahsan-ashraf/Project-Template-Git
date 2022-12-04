using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NextEdgeGames {
    public class Store : Menu {

        [Header("Inspector Assigned")]
        [SerializeField] private TextMeshProUGUI text_Title;
        [SerializeField] private Button button_Back;

        public override UIManager.MenuEnum Type => UIManager.MenuEnum.Store;

        private void Start() {
            button_Back.onClick.AddListener(OnClickBackButton);
        }
        private void OnClickBackButton() {
            UIManager.instance.Back();
        }
        public override void SetActive(bool setActive) {
            gameObject.SetActive(setActive);
            text_Title.gameObject.SetActive(setActive);
            button_Back.gameObject.SetActive(setActive);
        }
    }
}