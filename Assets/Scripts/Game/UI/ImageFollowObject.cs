using UnityEngine;

namespace Game.UI {
    public class ImageFollowObject : MonoBehaviour {
        private Camera _mainCamera;
        private Camera MainCamera => _mainCamera ??= Camera.main;

        private void Update() {
            if (!Main.IsGameStarted || !MainCamera) {
                return;
            }
            Vector3 screenPosition = _mainCamera.WorldToScreenPoint(PlayerAimPosition.Instance.transform.position);
            transform.position = screenPosition;
        }
    }
}