using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Game {
    public class WeaponsPanel : MonoBehaviour {
        [SerializeField] private List<WeaponIcon> _icons;

        private void Awake() {
            foreach (var icon in _icons) {
                icon.OnClick = OnIconClick;
            }
            
            MessageBroker.Default.Receive<PlayerWeaponSelectedSignal>().Subscribe(OnWeaponSelectedSignal);
        }

        private void OnIconClick(WeaponIcon icon) {
            Player.Instance.Weapons.TrySelectWeapon(icon.Type);
        }

        private void OnWeaponSelectedSignal(PlayerWeaponSelectedSignal signal) {
            foreach (var icon in _icons) {
                icon.Redraw();
            }
        }
        
    }
}
