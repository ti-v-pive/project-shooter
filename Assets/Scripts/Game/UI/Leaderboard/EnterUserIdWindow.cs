using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [SerializeField] private Button _buttonAccept;
        [SerializeField] private Button _buttonReject;

        private Response _response;

        protected override void Awake() {
            base.Awake();
            _buttonAccept.onClick.AddListener(OnAcceptClick);
            _buttonReject.onClick.AddListener(OnRejectClick);
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

        private void OnAcceptClick() => _response = Response.Accept;
        private void OnRejectClick() => _response = Response.Reject;

#if UNITY_EDITOR
        [ContextMenu("TestUsername")]
        private void TestNickname() {
            var result = UsernameManager.TryGetUsername();
            result.ToObservable().Subscribe(Debug.Log);
        }

        [ContextMenu("TestAddScore")]
        private void TestAddScore() {
            LeaderboardManager.AddScore(1000);
        }
        
        [ContextMenu("TestGetScore")]
        private void TestGetScore() {
            LeaderboardManager.GetTopScores(10).ToObservable().Subscribe(ScoreCallback);
        }
        
        private static void ScoreCallback(List<PlayerLeaderboardEntry> list) {
            foreach (var entry in list) {
                Debug.Log($"DisplayName: {entry.DisplayName}, Position: {entry.Position}, Score: {entry.StatValue}");
            }
        }
#endif
    }
}