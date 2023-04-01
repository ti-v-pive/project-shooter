namespace Game {
    public class PlayerHealth : Health {
        public override void TakeDamage(float damage) {
            if (Main.Instance.Modifications.IsInvulnerable) {
                return;
            }
            
            base.TakeDamage(damage);
        }
    }
}
