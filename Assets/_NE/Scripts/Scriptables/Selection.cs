using UnityEngine;

namespace NextEdgeGames {
    [CreateAssetMenu(fileName = "Selection", menuName = "Scriptable Objects/Selection")]
    public class Selection : ScriptableObject {
        [System.Serializable] public class LevelSelection {
            public int modeNo = 1;
            public int levelNo = 1;
        }

        public LevelSelection levelSelection;

        private void OnValidate() {
            levelSelection.modeNo = levelSelection.modeNo <= 0 ? 1 : levelSelection.modeNo;
            levelSelection.levelNo = levelSelection.levelNo <= 0 ? 1 : levelSelection.levelNo;
        }

    }
}