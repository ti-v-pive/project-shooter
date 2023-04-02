using UnityEngine;

namespace Game.Animation {
    public class RigidbodyToAnimatorMovement : MonoBehaviour {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Animator animator;
        [SerializeField] private string horizontalVelocityParameter = "HorizontalVelocity";
        [SerializeField] private string verticalVelocityParameter = "VerticalVelocity";
        [SerializeField] private string speedParameter = "Speed";

        private Transform _transform;
        private Transform Transform => _transform ??= rb.transform;

        private void Update() {
            Vector3 localVelocity = Transform.InverseTransformDirection(rb.velocity);

            float horizontalVelocity = localVelocity.x;
            float verticalVelocity =localVelocity.z;

            animator.SetFloat(horizontalVelocityParameter, horizontalVelocity);
            animator.SetFloat(verticalVelocityParameter, verticalVelocity);
        }
    }
}