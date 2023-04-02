using UnityEngine;
using UnityEngine.UI;

namespace Game {
    public class GameItemsPanel : MonoBehaviour {
        [SerializeField] private GameItemType _type;
        [SerializeField] private Text _countLabel;

        private static GameItemsManager Inventory => Main.Instance.Inventory;

        private void Awake() {
            Inventory.Get(_type).OnChangeListeners += OnChange;
            Redraw();
        }

        private void OnDestroy() {
            Inventory.Get(_type).OnChangeListeners -= OnChange;
        }

        private void OnChange() {
            if (!gameObject) {
                OnDestroy();
                return;
            }

            Redraw();
        }

        private void Redraw() {
            var count = Inventory.Get(_type).Count;

            if (_countLabel) {
                _countLabel.text = count.ToString();
            }
        }
    }
}
