using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.UI {
    public class LoadingImageController : MonoBehaviour {
        public Image loadingImage;

        private void Start() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            if (loadingImage != null) {
                loadingImage.gameObject.SetActive(false);
            }
        }

        private void OnDestroy() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}