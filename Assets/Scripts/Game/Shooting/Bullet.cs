using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class Bullet : MonoBehaviour {
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;
        
        [Tooltip("Через сколько секунд умрет пуля, если никуда не попадет")]
        [SerializeField] private float _lifeTime = 5;
        
        private float _damage;

        private void Awake() {
            Destroy(gameObject, _lifeTime);
        }

        private void OnCollisionEnter(Collision collision) {
            var target = collision.gameObject.GetComponent<Health>();
            if (!target) {
                return;
            }
            
            target.TakeDamage(_damage);
            Destroy(gameObject);
        }

        public void SetForce(Vector3 direction, float force) {
            transform.forward = direction.normalized;
            _rigidbody.AddForce(direction.normalized * force, ForceMode.Impulse);
        }
        
        public void SetDamage(float damage) {
            _damage = damage;
        }

        public void Ignore(IEnumerable<Collider> targets) {
            foreach (var target in targets) {
                Physics.IgnoreCollision(_collider, target);
            }
        }
    }
}
