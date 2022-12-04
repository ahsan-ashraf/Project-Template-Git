using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NextEdgeGames {
    public class LevelSelection : Menu {

        [Header("Inspector Assigned")]
        [SerializeField] private TextMeshProUGUI text_Title;
        [SerializeField] private Button button_Play;
        [SerializeField] private Button button_Back;
        [SerializeField] private UIManager.MenuEnum nextMenu;

        public override UIManager.MenuEnum Type => UIManager.MenuEnum.LevelSelection;

        private void Start() {
            button_Play.onClick.AddListener(OnClickPlayButton);
            button_Back.onClick.AddListener(OnClickBackButton);
        }
        private void OnClickPlayButton() {
            if (nextMenu != UIManager.MenuEnum.None)
                UIManager.instance.OpenMenu(nextMenu);
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