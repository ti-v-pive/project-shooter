using System;

namespace Game {
    public class GameItem {
        public GameItemType Type { get; }
        public int Count { get; protected set; }
        public event Action OnChangeListeners;

        public GameItem(GameItemType type) {
            Type = type;
        }
        
        public void Clear() => SetCount(0);
        public void Add(int value) => SetCount(Count + value);
        public void Spend(int value) => SetCount(Count - value);
        
        public void SetCount(int value) {
            if (value == Count) {
                return;
            }

            Count = value;
            OnChange();
        }

        private void OnChange() {
            OnChangeListeners?.Invoke();
        }
    }
}
