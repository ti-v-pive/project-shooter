using UniRx;
using UnityEngine;

namespace Game {
    public class ShootSignal {
        public CreatureType OwnerType;
    }

    public class Weapon : MonoBehaviour {

        public CreatureType OwnerType = CreatureType.Bot;
        
        [SerializeField] private float _damage;

        [Header("Bullet")]
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _bulletSpawner;
        
        [Header("Shoot")]
        [SerializeField] private Shooter _shooter;

        private bool IsPlayer => OwnerType == CreatureType.Player;
        private Transform _transform;
        
        private void Awake() {
            _shooter.OnShootListeners += Shoot;
        }

        private void Shoot() {
            _transform ??= transform;
            
            var direction = _transform.forward + _shooter.Spread;
            var damage = IsPlayer && Main.Instance.Modifications.IsDoubleDamage ? 2.0f * _damage : _damage;
            
            var bullet = Instantiate(_bulletPrefab, _bulletSpawner.position, _transform.rotation);
            bullet.SetForce(direction, _shooter.Force);
            bullet.SetDamage(damage);

            MessageBroker.Default.Publish(new ShootSignal { OwnerType = OwnerType });
        }

    }
}
