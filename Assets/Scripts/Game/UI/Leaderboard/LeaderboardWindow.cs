using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using UniRx;

namespace Game.UI.Leaderboard {
    public class LeaderboardWindow : MonoBehaviourSingleton<LeaderboardWindow> {
        private enum Response {
            None,
            Accept,
            Reject
        }
        
        [SerializeField] private Button _buttonClose;

        private Response _response;

        protected override void Awake() {
            base.Awake();
            
            if (_buttonClose) {
                _buttonClose.onClick.AddListener(OnCloseClick);
            }
            
            HideInternal();
        }

        public async Task Show() {
            _response = Response.None;
            ShowInternal();
            await WaitForButton().ToObservable();
            HideInternal();
        }
        
        private IEnumerator WaitForButton() {
            while (_response == Response.None) {
                yield return null;
            }
        }

        private void ShowInternal() => gameObject.SetActive(true);
        private void HideInternal() => gameObject.SetActive(false);

        private void OnCloseClick() => _response = Response.Reject;
    }
}