using UnityEngine;

namespace NextEdgeGames {
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/GameSettings")]
    public class GameSettings : ScriptableObject {

        #region Singleton

        private static GameSettings instance;

        public static GameSettings Instance { 
            get {
                if (instance == null) {
                    instance = Resources.Load("GameSettings") as GameSettings;
                    if (instance == null) {
                        Debug.LogError("No GameSettings Scriptable Object found in Resources Folder. Make sure there is one.");
                    }
                }
                return instance;
            }
        }
        #endregion

        [SerializeField] private DB db;
        [SerializeField] private Selection selection;

        public Selection Selection { get => selection; }
        public DB DB { get => db; }
    }
}