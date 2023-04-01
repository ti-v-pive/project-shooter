using UnityEngine;
using Utils;

namespace Game {
    public class ShootSignal : Signal {}

    public class Weapon : MonoBehaviour {
        [SerializeField] private float _damage;

        [Header("Bullet")]
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _bulletSpawner;
        
        [Header("Shoot")]
        [SerializeField] private Shooter _shooter;
        
        private Transform _transform;

        public readonly ShootSignal ShootSignal = new();
        
        private void Awake() {
            _shooter.OnShootListeners += Shoot;
        }

        private void Shoot() {
            _transform ??= transform;
            
            var direction = _transform.forward + _shooter.Spread;
            
            var bullet = Instantiate(_bulletPrefab, _bulletSpawner.position, _transform.rotation);
            bullet.SetForce(direction, _shooter.Force);
            bullet.SetDamage(_damage);
            
            ShootSignal.Fire();
        }

    }
}
