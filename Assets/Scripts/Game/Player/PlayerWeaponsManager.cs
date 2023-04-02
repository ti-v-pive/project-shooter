using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Utils;

namespace Game {
    public class PlayerWeaponSelectedSignal {}

    public class PlayerWeaponsManager : MonoBehaviourSingleton<PlayerWeaponsManager> {
        [SerializeField] private PlayerHand _rightHand;
        [SerializeField] private List<Weapon> _weapons;

        public List<Weapon> All => _weapons;
        public Weapon GetByType(WeaponType type) => _weapons.FirstOrDefault(w => w.Type == type);

        private void Start() {
            TrySelectWeapon(0);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                TrySelectWeapon(0);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                TrySelectWeapon(1);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                TrySelectWeapon(2);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                TrySelectWeapon(3);
            }
        }

        private void TrySelectWeapon(int index) {
            var weapon = index < _weapons.Count ? _weapons[index] : null;
            Select(weapon);
        }

        private void Select(Weapon weapon) {
            if (!weapon) {
                return;
            }
            
            _rightHand.SetWeapon(weapon);
            MessageBroker.Default.Publish(new PlayerWeaponSelectedSignal());
        }
        
        public void TrySelectWeapon(WeaponType type) {
            var weapon = GetByType(type);
            Select(weapon);
        }

        public void AddWeapon(Weapon weapon) {
            GetByType(weapon.Type)?.AddBulletsCount(weapon.BulletsCount);
        }

        public void AddBullets(WeaponType type, int count) {
            GetByType(type)?.AddBulletsCount(count);
        }
        

    }
}
