using System.Collections.Generic;
using UnityEngine;

namespace NextEdgeGames {
    [CreateAssetMenu(fileName = "DB", menuName = "Scriptable Objects/DB")]
    public class DB : ScriptableObject {

        [System.Serializable] public class ModeData {
            [System.Serializable] public class LevelData {
                [Header("Inspector Assigned")]
                public Sprite image;
                public int levelNo;
                public bool unlockByDefault;

                [Header("Runtime Information")]
                public int modeNo;
                public string prefID;
                public bool manualUnlock;

                public bool Unlocked { get => PlayerPrefs.GetInt(prefID, 0) == 1 || unlockByDefault || manualUnlock; }  // also add iap checks

                public void Validate(int modeNo, bool manualUnlock) {
                    this.modeNo = modeNo; 
                    this.manualUnlock = manualUnlock;
                    this.prefID = "M" + modeNo + "L" + levelNo;
                }
                public void SetLock(bool lockState) {
                    PlayerPrefs.SetInt(prefID, lockState ? 0 : 1);
                }
            }

            [Header("Inspector Assigned")]
            public Sprite image;
            public int modeNo;
            public bool unlockByDefault;
            public bool isComingSoon;
            public List<LevelData> levelsData;

            [Header("Runime Information")]
            public string prefID;
            public bool manualUnlock;

            public int TotalLevels { get => levelsData.Count; }
            public bool Unlocked { get => PlayerPrefs.GetInt(prefID, 0) == 1 || unlockByDefault || manualUnlock; } // also add iap checks

            public void Validate(bool manualUnlockAllModes, bool manualUnlockAllLevels) {
                prefID = "M" + modeNo;
                this.manualUnlock = manualUnlockAllModes;
                for (int i = 0; i < levelsData.Count; i++) {
                    levelsData[i].levelNo = i + 1;
                    levelsData[i].Validate(modeNo, manualUnlockAllLevels);
                }
            }
            public void SetLockState(bool lockState) {
                PlayerPrefs.SetInt(prefID, lockState ? 0 : 1);
            }
        }

        public bool manualUnlockAllModes;
        public bool manualUnlockAllLevels;
        public List<ModeData> modeData;

        private void OnValidate() {
            for (int i = 0; i < modeData.Count; i++) {
                modeData[i].modeNo = i + 1;
                modeData[i].Validate(manualUnlockAllModes, manualUnlockAllLevels);
            }
        }
    }
}