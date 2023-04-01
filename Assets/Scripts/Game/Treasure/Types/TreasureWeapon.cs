using UnityEngine;

namespace Game {
    public class TreasureWeapon : Treasure {
        [SerializeField] private Weapon _weapon;
        
        public override void Accept() {
            Main.Instance.Player.Weapons.AddWeapon(_weapon);
        }
    }
}
