using UnityEngine;

namespace Game {
    public class TreasurePlayerHealth : Treasure {
        [SerializeField] private float _health;
        
        public override void Accept() {
            Main.Instance.Player.Health.TakeHeal(_health);
        }
    }
}
