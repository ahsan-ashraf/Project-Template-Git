using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NextEdgeGames {
    public class ModeSelection : Menu {

        private List<ModeButton> modeButtons;

        [Header("Inspector Assigned")]
        [SerializeField] private TextMeshProUGUI text_Title;
        [SerializeField] private Button button_Next;
        [SerializeField] private Button button_Back;
        [SerializeField] private ScrollRect scrollRect_Modes;
        [SerializeField] private UIManager.MenuEnum nextMenu;

        public override UIManager.MenuEnum Type => UIManager.MenuEnum.ModeSelection;

        private void Awake() {
            modeButtons = scrollRect_Modes.content.GetComponentsInChildren<ModeButton>().ToList();
        }
        private void Start() {
            button_Next.onClick.AddListener(OnClickNextButton);
            button_Back.onClick.AddListener(OnClickBackButton);

            foreach (ModeButton modeButton in modeButtons) {
                modeButton.Init(this);
            }
        }
        private void OnClickNextButton() {
            if (nextMenu != UIManager.MenuEnum.None)
                UIManager.instance.OpenMenu(nextMenu);
        }
        private void OnClickBackButton() {
            UIManager.instance.Back();
        }
        private void DeSelectAllModes() {
            foreach (ModeButton modeButton in modeButtons) {
                modeButton.SetSelect(false);
            }
        }
        public override void SetActive(bool setActive) {
            gameObject.SetActive(setActive);
            text_Title.gameObject.SetActive(setActive);
            button_Back.gameObject.SetActive(setActive);
        }

        public void SelectMode(ModeButton modeButton) {
            if (modeButton.Unlocked) {
                DeSelectAllModes();
                modeButton.SetSelect(true);
            } else {
                // open mode locked dialogue
            }
        }
    }
}