using UnityEngine;

namespace NextEdgeGames {
    public abstract class Dialogue : UIPanel {
        public abstract UIManager.DialogueEnum Type { get; }
    }
}