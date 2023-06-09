﻿using UnityEngine;

namespace Game {
    public class PlayerHand : MonoBehaviour {
        [SerializeField] private Transform _container;
        [SerializeField] private Transform _ignore;

        public Weapon Weapon { get; private set; }

        public void SetWeapon(Weapon weaponPrefab) {
            if (Weapon) {
                Destroy(Weapon.gameObject);
                Weapon = null;
            }

            if (!weaponPrefab) {
                return;
            }
            
            Weapon = Instantiate(weaponPrefab, _container);
            Weapon.SetOwner(CreatureType.Player);
            Weapon.Ignore(_ignore);
        }
        
        private void Update() {
            if (!Weapon) {
                return;
            }
            
            if (Input.GetMouseButtonDown(0)) {
                Weapon.TryShoot();
            }

            if (Input.GetMouseButton(0) && Weapon.IsAutomatic) {
                Weapon.TryShoot();
            }
        }
    }
}
