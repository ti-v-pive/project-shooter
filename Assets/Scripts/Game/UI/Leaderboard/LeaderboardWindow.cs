using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
        [SerializeField] private List<LeaderPlayer> _players;
        [SerializeField] private LeaderPlayer _myStat;
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
            var scores = await LeaderboardManager.GetTopScores();
            for (int i = 0; i < _players.Count; i++) {
                var entry = scores.FirstOrDefault(e => e.Position == i);
                var player = _players[i];
                player.Init(entry);
            }
            var myStat = scores.FirstOrDefault(s => s.PlayFabId == PlayFabLogin.LoginResult.PlayFabId);
            if (myStat != null) {
                _myStat.Init(myStat);
            } else {
                var nickname = await UserManager.TryGetUsername();
                _myStat.Init(nickname, Main.Instance.Inventory.Coins.Count);
            }
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