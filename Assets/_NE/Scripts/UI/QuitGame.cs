using UnityEngine;

namespace NextEdgeGames {
    public class QuitGame : Dialogue {

        private UIManager.DialogueEnum type = UIManager.DialogueEnum.QuitGame;

        public override UIManager.DialogueEnum Type => type;

        public override void SetActive(bool setActive) {
            gameObject.SetActive(setActive);
        }
    }
}