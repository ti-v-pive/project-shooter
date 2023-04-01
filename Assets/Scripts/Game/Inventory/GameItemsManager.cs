using System.Collections.Generic;

namespace Game {
    public class GameItemsManager {
        private readonly Dictionary<GameItemType, GameItem> _values = new ();

        public GameItem Get(GameItemType type) {
            if (!_values.ContainsKey(type)) {
                _values.Add(type, new GameItem(type));
            }

            return _values[type];
        }
        
        public GameItem Exp => Get(GameItemType.Exp);
        public GameItem Coins => Get(GameItemType.Coins);
        
    }
}
