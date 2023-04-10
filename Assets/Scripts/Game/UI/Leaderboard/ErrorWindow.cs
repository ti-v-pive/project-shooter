using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Game.UI.Leaderboard {
    public class ErrorWindow : MonoBehaviourSingleton<ErrorWindow> {

        [SerializeField] private TMP_Text _errorText;
        [SerializeField] private Button _buttonAccept;
        
        private void Awake() {
            _buttonAccept.onClick.AddListener(HideInternal);
        }
        
        private void ShowInternal() => gameObject.SetActive(true);
        private void HideInternal() => gameObject.SetActive(false);

        public async Task Show(string errorText) {
            _errorText.text = errorText;
            ShowInternal();
        }
    }
}