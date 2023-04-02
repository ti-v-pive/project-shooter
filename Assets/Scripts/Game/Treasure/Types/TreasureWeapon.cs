using UnityEngine;

namespace Game {
    public class TreasureWeapon : Treasure {
        [SerializeField] private Weapon _weapon;
        
        public override void Accept() {
            Player.Instance.Weapons.AddWeapon(_weapon);
        }
    }
}
