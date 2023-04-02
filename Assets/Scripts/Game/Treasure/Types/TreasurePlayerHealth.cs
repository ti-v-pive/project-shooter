using UnityEngine;

namespace Game {
    public class TreasurePlayerHealth : Treasure {
        [SerializeField] private float _health;
        
        public override void Accept() {
            Player.Instance.Health.TakeHeal(_health);
        }
    }
}
