using UnityEngine;
using Utils;

namespace Game {
    public class PlayerAimPosition : MonoBehaviourSingleton<PlayerAimPosition> {
        public float yOffset = 0.5f;
        
        private Camera _mainCamera;

        protected override void Awake() {
            base.Awake();
            _mainCamera = Camera.main;
        }

        private void Update() {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, new Vector3(0, yOffset, 0));

            if (!plane.Raycast(ray, out var distance)) {
                return;
            }
            Vector3 hitPoint = ray.GetPoint(distance);
            transform.position = new Vector3(hitPoint.x, yOffset, hitPoint.z);
        }
    }
}