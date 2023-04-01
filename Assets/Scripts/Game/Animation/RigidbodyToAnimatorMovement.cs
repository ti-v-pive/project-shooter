using UnityEngine;

namespace Game.Animation {
    public class RigidbodyToAnimatorMovement : MonoBehaviour {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Animator animator;
        [SerializeField] private string horizontalVelocityParameter = "HorizontalVelocity";
        [SerializeField] private string verticalVelocityParameter = "VerticalVelocity";
        [SerializeField] private float velocityMultiplier = 1.0f;

        private void Update() {
            Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);

            float horizontalVelocity = localVelocity.x * velocityMultiplier;
            float verticalVelocity = localVelocity.z * velocityMultiplier;

            animator.SetFloat(horizontalVelocityParameter, horizontalVelocity);
            animator.SetFloat(verticalVelocityParameter, verticalVelocity);
        }
    }
}