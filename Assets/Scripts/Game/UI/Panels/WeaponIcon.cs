using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
    public class WeaponIcon : MonoBehaviour {
        [SerializeField] private WeaponType _type;
        [SerializeField] private Button _button;

        public WeaponType Type => _type;
        public Action<WeaponIcon> OnClick;

        private void Awake() {
            _button.onClick.AddListener(() => OnClick?.Invoke(this));
        }

        public void Redraw() {
            
        }

    }
}
