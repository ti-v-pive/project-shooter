using UnityEngine;

namespace Game {
    public abstract class Treasure : MonoBehaviour {
        public abstract void Accept();

        public void Die() {
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other) {
            var target = other.GetComponent<TreasureCollector>();
            if (!target) {
                return;
            }
            
            Accept();
            Die();
        }
    }
}
