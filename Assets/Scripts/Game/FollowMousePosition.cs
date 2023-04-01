using UnityEngine;

namespace Game {
    public class FollowMousePosition : MonoBehaviour {
        public Camera mainCamera;
        public float yOffset = 0.5f;

        private void Update() {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, new Vector3(0, yOffset, 0));

            if (!plane.Raycast(ray, out var distance)) {
                return;
            }
            Vector3 hitPoint = ray.GetPoint(distance);
            transform.position = new Vector3(hitPoint.x, yOffset, hitPoint.z);
        }
    }
}