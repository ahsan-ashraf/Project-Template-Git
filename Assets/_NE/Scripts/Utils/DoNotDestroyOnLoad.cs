using UnityEngine;

namespace NextEdgeGames {
    public class DoNotDestroyOnLoad : MonoBehaviour {

        #region Monobehaviour Messages
        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }
        private void OnValidate() {
            gameObject.name = "[Do Not Destroy On Load]";
        }
        #endregion
    }
}