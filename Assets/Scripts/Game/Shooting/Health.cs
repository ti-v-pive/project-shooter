using System;
using UnityEngine;

namespace Game {
    public class Health : MonoBehaviour {
        [SerializeField] private Transform _objectToDestroy;
        [SerializeField] private float _health;

        public bool IsDead { get; private set; }
        public event Action OnChangeListeners;

        public void TakeHeal(float heal) => TryChangeHealth(heal);
        public virtual void TakeDamage(float damage) => TryChangeHealth(-damage);

        private void TryChangeHealth(float addition) {
            if (IsDead) {
                return;
            }

            _health += addition;
            OnChangeListeners?.Invoke();
            
            if (_health > 0) {
                return;
            }
            
            Die();
        }

        private void Die() {
            IsDead = true;
            PlayDieEffect();
        }

        private void PlayDieEffect() {
            if (_objectToDestroy) {
                Destroy(_objectToDestroy.gameObject);
                return;
            }
            
            Destroy(gameObject);
        }
    }
}
