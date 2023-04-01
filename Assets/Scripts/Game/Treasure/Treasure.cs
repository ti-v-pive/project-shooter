using UnityEngine;

namespace Game {
    public abstract class Treasure : MonoBehaviour {
        public abstract void Accept();

        public void Die() {
            Destroy(gameObject);
        }
    }
}
