using UnityEngine;

namespace Game.Movement {
    public class PlayerMovement : MonoBehaviour {
        public float moveSpeed = 5.0f;
        public Rigidbody rb;
        
        private Vector3 _movement;

        private void Update() {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            _movement = new Vector3(moveX, 0, moveY).normalized;
        }

        private void FixedUpdate() {
            rb.velocity = _movement * moveSpeed;
        }
    }
}