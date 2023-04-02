using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
    public class PlayerModificationPanel : MonoBehaviour {
        [SerializeField] private ModificationType _type;
        [SerializeField] private Slider _slider;

        private static ModificationsManager Modifications => Main.Instance.Modifications;

        private bool _isVisible;
        private float _max;
        
        private void Awake() {
            SetIsVisible(false);
        }

        private void Update() {
            var isModificationOn = Modifications.IsActive(_type);
            if (isModificationOn != _isVisible) {
                SetIsVisible(isModificationOn);

                if (!_isVisible) {
                    return;
                }
                
                _max = Modifications.TimeLeft(_type);
                _slider.value = 1;
                return;
            }

            if (!_isVisible) {
                return;
            }

            var left = Modifications.TimeLeft(_type);
            var p = _max > 0 ? Math.Clamp(left / _max, 0f, 1f) : 0f;
            _slider.value = p;
        }

        private void SetIsVisible(bool isVisible) {
            _isVisible = isVisible;
            _slider.gameObject.SetActive(isVisible);
        }

    }
}
