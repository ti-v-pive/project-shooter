using UnityEngine;

namespace Game {
    public class TreasureItem : Treasure {
        [SerializeField] private GameItemType _type;
        [SerializeField] private int _value;
        
        public override void Accept() {
            Main.Instance.Inventory.Get(_type).Add(_value);
        }
    }
}
