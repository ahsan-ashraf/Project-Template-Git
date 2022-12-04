using UnityEngine;

namespace NextEdgeGames {
    public abstract class Menu : UIPanel {

        [Header("Inherited Fields")]
        [SerializeField] private bool useBGImage = true;

        public bool UseBGImage { get => useBGImage; }
        public abstract UIManager.MenuEnum Type { get; }
    }
}