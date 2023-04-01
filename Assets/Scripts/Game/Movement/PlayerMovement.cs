using UnityEngine;

namespace Game.Movement {
    public class PlayerMovement : MonoBehaviour {
        public float moveSpeed = 5.0f;

        private Rigidbody _rb;
        private Vector3 _movement;

        private void Start() {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update() {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            _movement = new Vector3(moveX, 0, moveY).normalized;
        }

        private void FixedUpdate() {
            _rb.velocity = _movement * moveSpeed;
        }
    }
}