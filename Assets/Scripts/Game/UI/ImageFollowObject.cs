using UnityEngine;

namespace Game.UI {
    public class ImageFollowObject : MonoBehaviour {
        private Camera _mainCamera;

        private void Start() {
            _mainCamera ??= Camera.main;
        }

        private void Update() {
            Vector3 screenPosition = _mainCamera.WorldToScreenPoint(PlayerAimPosition.Instance.transform.position);
            transform.position = screenPosition;
        }
    }
}