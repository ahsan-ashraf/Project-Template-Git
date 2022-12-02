using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NextEdgeGames {
    public class LoadingUi : MonoBehaviour {

        #region Serializable Nested Classes
        [System.Serializable] public class Event {
            public UnityEvent OnLoadingScreenActivate;
            public UnityEvent OnLoadingScreenDeActivate;
        }
        #endregion

        #region Public Static Fields
        public static LoadingUi instance;
        #endregion

        #region Private Serialized Fields
        [Header("Inspector Assigned")]
        [SerializeField] private Transform container;
        [SerializeField] private Text text_Title;
        [SerializeField] private Text text_Progress;
        [SerializeField] private Image image_Fill;
        [SerializeField] private Event events;
        #endregion

        #region Public Properties
        public Event Events { get => events; }
        #endregion

        #region Monobehaviour Messages
        private void Awake() {
            #region Singleton
            if (instance == null) {
                instance = this;
            }
            else {
                Destroy(gameObject);
            }
            #endregion
        }
        #endregion

        #region Private Coroutines
        private IEnumerator C_LoadScene(string sceneToLoad, string title = "Loading...", bool disableLoadingWhenDone = true) {
            ActivateLoadingScreen();
            yield return null;
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
            while (!operation.isDone) {
                float prog = Mathf.Clamp01(operation.progress / 0.9f);
                UpdateProgress(prog, title);
                yield return null;
            }
            if (disableLoadingWhenDone) {
                DeActivateLoading();
            }
        }
        private IEnumerator C_LoadScene(int sceneToLoad, string title = "Loading...", bool disableLoadingWhenDone = true) {
            ActivateLoadingScreen();
            yield return null;
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
            while (!operation.isDone) {
                float prog = Mathf.Clamp01(operation.progress / 0.9f);
                UpdateProgress(prog, title);
                yield return null;
            }
            if (disableLoadingWhenDone) {
                DeActivateLoading();
            }
        }
        #endregion

        #region Private Methods
        private void Clear() {
            text_Title.text = "Loading...";
            text_Progress.text = "00%";
            image_Fill.fillAmount = 0f;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Takes name of scene to load, loading title, and boolean to disable the loading screen when operation is completes.
        /// </summary>
        /// <param name="sceneToLoad"></param>
        /// <param name="title"></param>
        /// <param name="disableLoadingWhenDone"></param>
        public void LoadScene(string sceneToLoad, string title = "Loading...", bool disableLoadingWhenDone = true) {
            StartCoroutine(C_LoadScene(sceneToLoad, title, disableLoadingWhenDone));
        }

        /// <summary>
        /// Takes index of scene to load, loading title, and boolean to disable the loading screen when operation is completes.
        /// </summary>
        /// <param name="sceneToLoad"></param>
        /// <param name="title"></param>
        /// <param name="disableLoadingWhenDone"></param>
        public void LoadScene(int sceneToLoad, string title = "Loading...", bool disableLoadingWhenDone = true) {
            StartCoroutine(C_LoadScene(sceneToLoad, title, disableLoadingWhenDone));
        }

        /// <summary>
        /// Sets progress and title of loading screen.
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="title"></param>
        public void UpdateProgress(float progress, string title) {
            image_Fill.fillAmount = progress;
            text_Title.text = title;
            text_Progress.text = (progress * 100f).ToString("00") + "%";
        }

        /// <summary>
        /// Activates the loading screen.
        /// </summary>
        public void ActivateLoadingScreen() {
            Clear();
            container.gameObject.SetActive(true);
            Events.OnLoadingScreenActivate.Invoke();
        }

        /// <summary>
        /// Deactivates the loading screen.
        /// </summary>
        public void DeActivateLoading() {
            container.gameObject.SetActive(false);
            Clear();
            Events.OnLoadingScreenDeActivate.Invoke();
        }
        #endregion
    }
}