namespace Game {
    public class TreasureWin : Treasure {
        public override void Accept() {
            Main.Instance.Win();
        }
    }
}
