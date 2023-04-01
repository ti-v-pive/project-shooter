using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Utils;

namespace Game {
    public class PlayerWeaponSelectedSignal {}

    public class PlayerWeaponsManager : MonoBehaviourSingleton<PlayerWeaponsManager> {
        [SerializeField] private PlayerHand _rightHand;
        [SerializeField] private List<Weapon> _weapons; // temp

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
            if (!weapon) {
                return;
            }

            _rightHand.SetWeapon(weapon);
            MessageBroker.Default.Publish(new PlayerWeaponSelectedSignal());
        }

        public void AddWeapon(Weapon weapon) {
            _weapons.Add(weapon);
        }
        
    }
}
