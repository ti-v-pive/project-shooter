using System.Collections;
using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using UniRx;

namespace Game.UI.Leaderboard {
    public class EnterUserIdWindow : MonoBehaviourSingleton<EnterUserIdWindow> {
        private enum Response {
            None,
            Accept,
            Reject
        }
        
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private TMP_Text _errorText;
        [SerializeField] private Button _buttonAccept;
        [SerializeField] private Button _buttonReject;

        private Response _response;
        private bool _isNameValid;
        private bool _updateCommandCompleted;

        protected override void Awake() {
            base.Awake();
            _buttonAccept.onClick.AddListener(OnAcceptClick);
            _buttonReject.onClick.AddListener(OnRejectClick);
            HideInternal();
        }

        public async Task<string> Show() {
            _response = Response.None;
            _isNameValid = false;
            SetErrorText(string.Empty);
            
            ShowInternal();
            await WaitForName();
            var userId = _inputField.text;
            HideInternal();
            Debug.Log("5");
            return _response == Response.Accept ? userId : string.Empty;
        }
        
        private async Task WaitForName() {
            while (!_isNameValid) {
                _response = Response.None;
                bool ResponseChanged() => _response != Response.None;
                Debug.Log("1");
                await Observable.EveryUpdate()
                    .Where(_ => ResponseChanged())
                    .FirstOrDefault()
                    .ToTask();

                Debug.Log("2");
                if (_response != Response.Accept) {
                    return;
                }
                
                Debug.Log("3");

                NetLoadingImage.SetActive(true);
                _updateCommandCompleted = false;
                var displayNameRequest = new UpdateUserTitleDisplayNameRequest { DisplayName = _inputField.text };
                PlayFabClientAPI.UpdateUserTitleDisplayName(displayNameRequest, OnUpdateUserTitleDisplayName, OnUpdateUserTitleDisplayNameFailure);
                
                await Observable.EveryUpdate()
                    .Where(_ => _updateCommandCompleted)
                    .FirstOrDefault()
                    .ToTask();
                
                Debug.Log("4");
                NetLoadingImage.SetActive(false);
            }
        }
        
        private void OnUpdateUserTitleDisplayName(UpdateUserTitleDisplayNameResult updateUserTitleDisplayNameResult) {
            _updateCommandCompleted = true;
            _isNameValid = true;
            Debug.Log("UpdateUserTitleDisplayNameSuccess");
        }
        
        private void OnUpdateUserTitleDisplayNameFailure(PlayFabError error) {
            _updateCommandCompleted = true;
            _isNameValid = false;
            SetErrorText(error.ErrorMessage);
        }

        private void ShowInternal() => gameObject.SetActive(true);
        private void HideInternal() => gameObject.SetActive(false);

        private void OnAcceptClick() => _response = Response.Accept;
        private void OnRejectClick() => _response = Response.Reject;

        private void SetErrorText(string errorText) {
            _errorText.text = errorText;
            _errorText.gameObject.SetActive(!string.IsNullOrEmpty(errorText));
        }
    }
}