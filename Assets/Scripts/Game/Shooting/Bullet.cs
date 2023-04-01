using UnityEngine;

namespace Game {
    public class Bullet : MonoBehaviour {
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _lifeTime;
        [SerializeField] private float _damage;

        public Collider Collider => _collider;
        
        private void Awake() {
            Destroy(gameObject, _lifeTime);
        }

        private void OnCollisionEnter(Collision collision) {
            var target = collision.gameObject?.GetComponent<Health>();
            if (target) {
                target.TakeDamage(_damage);
            }

            Destroy(gameObject);
        }

        public void SetForce(Vector3 direction, float force) {
            transform.forward = direction.normalized;
            _rigidbody.AddForce(direction.normalized * force, ForceMode.Impulse);
        }
        
        public void SetDamage(float damage) {
            _damage = damage;
        }
    }
}
