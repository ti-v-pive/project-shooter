using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using UniRx;
using UnityEngine.Serialization;

namespace Game.UI.Leaderboard {
    public class LeaderboardWindow : MonoBehaviourSingleton<LeaderboardWindow> {
        private enum Response {
            None,
            Accept,
            Reject
        }
        
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _buttonClose;

        private Response _response;

        protected override void Awake() {
            base.Awake();
            _buttonClose.onClick.AddListener(OnCloseClick);
            HideInternal();
        }

        public async Task<string> Show() {
            _response = Response.None;
            ShowInternal();
            await WaitForButton().ToObservable();
            var userId = _inputField.text;
            HideInternal();
            return _response == Response.Accept ? userId : string.Empty;
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