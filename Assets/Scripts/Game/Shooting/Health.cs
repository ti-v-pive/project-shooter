using UnityEngine;

namespace Game {
    public class Health : MonoBehaviour {
        [SerializeField] private float _health;

        public void TakeDamage(float damage) {
            if (_health <= 0) {
                return;
            }

            _health -= damage;

            if (_health > 0) {
                return;
            }
            
            Die();
        }

        private void Die() {
            Destroy(gameObject);
        }
    }
}
