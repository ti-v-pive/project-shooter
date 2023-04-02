using UnityEngine;

namespace Game {
    public class PlayerHealth : Health {
        public override void TakeDamage(float damage, Collision collision) {
            if (Main.Instance.Modifications.IsShield) {
                return;
            }
            
            base.TakeDamage(damage, collision);
        }

        protected override void Die() {
            //base.Die();
            Main.Instance.Lose();
        }
    }
}
