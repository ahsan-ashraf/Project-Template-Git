using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NextEdgeGames {
    public class LevelButton : MonoBehaviour {

        private Button button;
        private LevelSelection levelSelection;

        [Header("Inspector Assigned")]
        [SerializeField] private TextMeshProUGUI text_LevelName;
        [SerializeField] private Image image_Level;
        [SerializeField] private Image image_Lock;
        [SerializeField] private Image image_Selector;

        [Header("Runtime Information")]
        [SerializeField] private int levelNo;
        [SerializeField] private DB.ModeData.LevelData levelData;

        public bool Unlocked { get => levelData.Unlocked; }

        private void Awake() {
            button = GetComponent<Button>();
        }
        private void Start() {
            button.onClick.AddListener(OnClickLevelButton);
        }

        private void OnValidate() {
            levelNo = transform.GetSiblingIndex() + 1;
            text_LevelName.text = "Level " + levelNo.ToString("00");
            levelData = GameSettings.Instance.DB.modeData[GameSettings.Instance.Selection.levelSelection.modeNo - 1].levelsData.Find(x => x.levelNo == levelNo);
        }

        private void OnClickLevelButton() {
            if (Unlocked) {
                //modeSelection.SelectMode(this);
            }
        }

        public void Init(LevelSelection levelSelection) {
            this.levelSelection = levelSelection;
            image_Lock.gameObject.SetActive(!Unlocked);
            int selectedLevelNo = GameSettings.Instance.Selection.levelSelection.levelNo;
            image_Selector.gameObject.SetActive(selectedLevelNo == levelNo);
        }
        public void SetSelect(bool select) {
            image_Selector.gameObject.SetActive(select);
            if (Unlocked && select) {
                image_Selector.gameObject.SetActive(true);
                GameSettings.Instance.Selection.levelSelection.levelNo = levelNo;
            }
        }
    }
}