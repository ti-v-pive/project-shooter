using UniRx;
using UnityEngine;

namespace Game {
    public class Health : MonoBehaviour {
        [SerializeField] private Transform _objectToDestroy;
        [SerializeField] private float _health;
        [SerializeField] private Effect _hitEffect;
        [SerializeField] private Animator _hitAnimation;
        [SerializeField] private Effect _destroyEffect;
        [SerializeField] private SoundEffect _hitSoundEffect;
        private static readonly int _hit = Animator.StringToHash("Hit");
        private static readonly int _random = Animator.StringToHash("Random");

        public bool IsDead { get; private set; }

        public virtual void TakeDamage(float damage, Collision collision) {
            if (IsDead) {
                return;
            }
            
            _health -= damage;
            
            _hitSoundEffect?.PlayNew();

            var point = collision.contacts[0].point;
            Vector3 collisionDirection = collision.contacts[0].normal;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, collisionDirection);

            if (_health > 0) {
                _hitEffect?.PlayNew(point, rotation);
                if (_hitAnimation) {
                    _hitAnimation.SetBool(_hit, true);
                    _hitAnimation.SetInteger(_random, Random.Range(0, 100));
                }
                return;
            }
            
            Die();
            _destroyEffect?.PlayNew(point, rotation);
        }

        protected virtual void Die() {
            IsDead = true;
            PlayDieEffect();
        }

        private void PlayDieEffect() {
            if (_objectToDestroy) {
                Destroy(_objectToDestroy.gameObject);
                return;
            }
            
            Destroy(gameObject);

            if (!gameObject.isStatic) {
                return;
            }
            MessageBroker.Default.Publish(new StaticObjectDestroy());
        }
    }

    public class StaticObjectDestroy {}
}
