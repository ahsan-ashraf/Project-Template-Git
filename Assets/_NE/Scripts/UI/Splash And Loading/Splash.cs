using System.Collections;
using UnityEngine;

namespace NextEdgeGames {
    public class Splash : MonoBehaviour {

        #region Serialized Private Fields
        [SerializeField] private float initialDelay = 3f;
        [SerializeField] private string defaultSceneToLoad = "Menu";
        #endregion

        #region Monobehaviour Messages
        private IEnumerator Start() {
            yield return new WaitForSeconds(initialDelay);
            LoadingUi.instance.LoadScene(defaultSceneToLoad);
        }
        private void OnValidate() {
            gameObject.name = "Splash";
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
        #endregion

    }
}