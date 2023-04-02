using UnityEngine;

namespace Game {
    public class TreasureBullets : Treasure {
        [SerializeField] private WeaponType _type;
        [SerializeField] private int _count;
        
        public override void Accept() {
            Player.Instance.Weapons.AddBullets(_type, _count);
        }
        
    }
}
