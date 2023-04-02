namespace Game {
    public class TreasureLose : Treasure {
        public override void Accept() {
            Main.Instance.Lose();
        }
    }
}
