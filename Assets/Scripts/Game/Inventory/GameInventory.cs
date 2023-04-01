using System.Collections.Generic;

namespace Game {
    public static class GameInventory {
        private static readonly Dictionary<GameItemType, GameItem> _values = new ();

        public static GameItem Get(GameItemType type) {
            if (!_values.ContainsKey(type)) {
                _values.Add(type, new GameItem(type));
            }

            return _values[type];
        }
        
        public static GameItem Lives => Get(GameItemType.Lives);
        public static GameItem Coins => Get(GameItemType.Coins);
        
    }
}
