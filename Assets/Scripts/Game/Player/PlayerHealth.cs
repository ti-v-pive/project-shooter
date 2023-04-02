namespace Game {
    public class PlayerHealth : Health {
        public override void TakeDamage(float damage) {
            if (Main.Instance.Modifications.IsShield) {
                return;
            }
            
            base.TakeDamage(damage);
        }

        protected override void Die() {
            //base.Die();
            Main.Instance.Lose();
        }
    }
}
