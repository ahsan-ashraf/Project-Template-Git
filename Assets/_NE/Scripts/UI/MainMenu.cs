using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NextEdgeGames {
    public class MainMenu : Menu {

        [Header("Inspector Assigned")]
        [SerializeField] private TextMeshProUGUI text_Title;
        [SerializeField] private Button button_Start;
        [SerializeField] private Button button_Back;
        [SerializeField] private Button button_RateUs;
        [SerializeField] private Button button_Privacy;
        [SerializeField] private Button button_MoreGames;
        [SerializeField] private UIManager.MenuEnum nextMenu;

        public override UIManager.MenuEnum Type => UIManager.MenuEnum.MainMenu;

        private void Start() {
            button_Start.onClick.AddListener(OnClickStartButton);
            button_Back.onClick.AddListener(OnClickBackButton);
            button_RateUs.onClick.AddListener(OnClickRateUsButton);
            button_Privacy.onClick.AddListener(OnClickPrivacyButton);
            button_MoreGames.onClick.AddListener(OnClickMoreGamesButton);
        }

        private void OnClickStartButton() {
            if (nextMenu != UIManager.MenuEnum.None)
                UIManager.instance.OpenMenu(nextMenu);
        }

        private void OnClickBackButton() {
            UIManager.instance.Back();
        }

        private void OnClickRateUsButton() {
            throw new System.NotImplementedException();
        }

        private void OnClickPrivacyButton() {
            throw new System.NotImplementedException();
        }

        private void OnClickMoreGamesButton() {
            throw new System.NotImplementedException();
        }

        public override void SetActive(bool setActive) {
            gameObject.SetActive(setActive);
            text_Title.gameObject.SetActive(setActive);
            button_Back.gameObject.SetActive(setActive);
        }
    }
}