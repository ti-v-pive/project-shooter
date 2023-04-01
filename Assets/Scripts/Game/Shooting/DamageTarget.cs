using UnityEngine;

namespace Game {
    public class DamageTarget : MonoBehaviour {
        public void TakeDamage(float damage) {
            Destroy(gameObject);
        }
    }
}
