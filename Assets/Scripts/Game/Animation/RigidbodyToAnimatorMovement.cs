using UnityEngine;
using UnityEngine.AI;

namespace Game.Animation {
    public class RigidbodyToAnimatorMovement : MonoBehaviour {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Animator animator;
        [SerializeField] private string horizontalVelocityParameter = "HorizontalVelocity";
        [SerializeField] private string verticalVelocityParameter = "VerticalVelocity";

        private Transform _transform;
        private Transform Transform {
            get {
                if (rb) {
                    return _transform ??= rb.transform;
                }
                if (navMeshAgent) {
                    return _transform ??= navMeshAgent.transform;
                }
                return transform;
            }
        }

        private void Update() {
            Vector3 velocity = Vector3.zero;
            if (rb) {
                velocity = rb.velocity;
            }
            if (navMeshAgent) {
                velocity = navMeshAgent.velocity;
            }
            Vector3 localVelocity = Transform.InverseTransformDirection(velocity);

            float horizontalVelocity = localVelocity.x;
            float verticalVelocity = localVelocity.z;

            animator.SetFloat(horizontalVelocityParameter, horizontalVelocity);
            animator.SetFloat(verticalVelocityParameter, verticalVelocity);
        }
    }
}