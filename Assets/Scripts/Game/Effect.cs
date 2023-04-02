using UnityEngine;

namespace Game {
    public class Effect : MonoBehaviour {
        [SerializeField] private float _lifeTime = 1f;

        private void Awake() {
            Destroy(gameObject, _lifeTime);
        }

        public void PlayNew(Vector3 position, Quaternion quaternion) {
            Instantiate(this, position, quaternion);
        }
    }
}
