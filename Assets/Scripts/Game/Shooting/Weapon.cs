using System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game {
    public class ShootSignal {
        public CreatureType OwnerType;
    }

    public class Weapon : MonoBehaviour {
        [SerializeField] private float _damage;
        [Header("Для инвентаря")]
        [SerializeField] private WeaponType _type;
        [Header("У ботов всегда Queue!")]
        [SerializeField] private WeaponShootType _shootType;
        
        [Header("Bullet")]
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _bulletSpawner;
        [Header("Скорость вылета пули")]
        [SerializeField] private float _bulletForce = 10;
        [Header("Количество пуль (-1 - бесконечно)")]
        [SerializeField] private int _bulletsCount = -1;
        
        [Header("Периодичность выстрелов")]
        [SerializeField] private float _shootCooldown = 0.1f;
        [Header("Разброс (для Queue)")]
        [SerializeField] private float _spreadMin = 0;
        [SerializeField] private float _spreadMax = 0.1f;
        [SerializeField] private float _spreadStep = 0.01f;
        [Header("Время сброса spread после последнего выстрела")]
        [SerializeField] private float _spreadCooldown = 1f;

        private float _spread;
        private float _lastShootTime;
        private Transform _transform;
        private CreatureType _ownerType;
        private Collider[] _collidersToIgnore;
        
        public bool IsPlayer => _ownerType == CreatureType.Player;
        public bool IsAutomatic => _shootType == WeaponShootType.Queue;

        public WeaponType Type => _type;
        public int BulletsCount => _bulletsCount;
        
        private void Awake() {
            _spread = _spreadMin;
        }

        public void TryShoot() {
            var timeSpend = Time.time - _lastShootTime;
            if (timeSpend > _spreadCooldown) {
                _spread = _spreadMin;
            }

            if (timeSpend < _shootCooldown) {
                return;
            }

            Shoot();
        }

        private void Shoot() {
            _lastShootTime = Time.time;
            _bulletsCount--;
            
            _transform ??= transform;
            
            var direction = _transform.forward + new Vector3 (Random.Range(-_spread, _spread), 0, 0);
            var damage = IsPlayer && Main.Instance.Modifications.IsDoubleDamage ? 2.0f * _damage : _damage;
            
            var bullet = Instantiate(_bulletPrefab, _bulletSpawner.position, _transform.rotation);
            bullet.SetForce(direction, _bulletForce);
            bullet.SetDamage(damage);
            bullet.Ignore(_collidersToIgnore);
            
            if (_spread < _spreadMax) {
                _spread = Math.Min(_spread + _spreadStep, _spreadMax);
            }

            MessageBroker.Default.Publish(new ShootSignal { OwnerType = _ownerType });
        }

        public void SetOwner(CreatureType owner) {
            _ownerType = owner;
        }

        public void Ignore(Transform parent) {
            _collidersToIgnore = parent.GetComponentsInChildren<Collider>();
        }

        public void AddBulletsCount(int count) {
            if (count < 0) {
                _bulletsCount = count;
                return;
            }

            _bulletsCount += count;
        }

    }
}
