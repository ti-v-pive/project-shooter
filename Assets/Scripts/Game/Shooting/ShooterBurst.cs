using Tics;
using UnityEngine;

namespace Game {
    public class ShooterBurst : Shooter {
        [Tooltip("Максимальное количество пуль в очереди (-1 - бесконечно)")]
        [SerializeField] private int _masShootCount = -1;
        [Tooltip("Периодичность выстрелов")]
        [SerializeField] private float _everyShootDelay = 0.2f;

        [Header("Разброс от min к max с течением времени")]
        [SerializeField] private float _spreadMin = 0;
        [SerializeField] private float _spreadMax = 0.1f;
        [SerializeField] private float _spreadDuration = 1f;
        [SerializeField] private EaseType _spreadEase = EaseType.SineIn;

        private float _spread;
        private int _shootCount;
        private readonly TicMan _tm = TicMan.Create();
        
        public override Vector3 Spread => new (Random.Range(-_spread, _spread), 0, 0);
        
        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                StartShoot();
                return;
            }
            
            if (Input.GetMouseButtonUp(0)) {
                StopShoot();
                return;
            }
        }

        private void StartShoot() {
            _shootCount = 0;
            _spread = _spreadMin;
            
            _tm.Reset();
            _tm.DoEvery(_everyShootDelay, TryShoot);
            _tm.FromTo(value => { _spread = value; }, _spreadMin, _spreadMax, _spreadDuration)
                .SetEase(_spreadEase);

            TryShoot();
        }

        private void TryShoot() {
            _shootCount++;

            if (_masShootCount > 0 && _shootCount > _masShootCount) {
                StopShoot();
                return;
            }
            
            Shoot();
        }

        private void StopShoot() {
            _tm.Reset();
        }

    }
}
