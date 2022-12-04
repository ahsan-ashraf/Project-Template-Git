using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NextEdgeGames {
    public class ModeButton : MonoBehaviour {

        private Button button;
        private ModeSelection modeSelection;

        [Header("Inspector Assigned")]
        [SerializeField] private TextMeshProUGUI text_ModeName;
        [SerializeField] private Image image_Mode;
        [SerializeField] private Image image_Lock;
        [SerializeField] private Image image_Selector;

        [Header("Runtime Information")]
        [SerializeField] private int modeNo;
        [SerializeField] private DB.ModeData modeData;

        public bool Unlocked { get => modeData.Unlocked; }

        private void Awake() {
            button = GetComponent<Button>();
        }
        private void Start() {
            button.onClick.AddListener(OnClickModeButton);
        }

        private void OnValidate() {
            modeNo = transform.GetSiblingIndex() + 1;
            text_ModeName.text = "MODE " + modeNo.ToString("00");
            modeData = GameSettings.Instance.DB.modeData.Find(x => x.modeNo == modeNo);
        }

        private void OnClickModeButton() {
            if (Unlocked) {
                modeSelection.SelectMode(this);
            }
        }

        public void Init(ModeSelection modeSelection) {
            this.modeSelection = modeSelection;
            image_Lock.gameObject.SetActive(!Unlocked);
            int selectedModeNo = GameSettings.Instance.Selection.levelSelection.modeNo;
            image_Selector.gameObject.SetActive(selectedModeNo == modeNo);
        }
        public void SetSelect(bool select) {
            image_Selector.gameObject.SetActive(select);
            if (Unlocked && select) {
                image_Selector.gameObject.SetActive(true);
                GameSettings.Instance.Selection.levelSelection.modeNo = modeNo;
            }
        }
    }
}