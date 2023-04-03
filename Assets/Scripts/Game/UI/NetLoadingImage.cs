using Utils;

namespace Game.UI {
    public class NetLoadingImage : MonoBehaviourSingleton<NetLoadingImage> {
        protected override void Awake() {
            base.Awake();
            SetActive(false);
        }

        public static void SetActive(bool value) {
            Instance.gameObject.SetActive(value);
        }
    }
}