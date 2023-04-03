using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayFab.ClientModels;
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
            if (scores == null) {
                HideInternal();
                return;
            }
            for (int i = 0; i < _players.Count; i++) {
                var entry = scores.FirstOrDefault(e => e.Position == i);
                var player = _players[i];
                player.Init(entry);
            }
            var nickname = await UserManager.TryGetUsername();
            var score = Main.Instance.Inventory.Exp.Count;
            var position = GetPositionForNewScore(scores, score);
            _myStat.Init(nickname, position, score);
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

        public static int GetPositionForNewScore(List<PlayerLeaderboardEntry> entries, int newScore) {
            entries = entries.OrderBy(x => x.Position).ToList();

            int newPosition = 1;
            foreach (PlayerLeaderboardEntry scoreData in entries) {
                if (newScore < scoreData.StatValue) {
                    newPosition++;
                } else {
                    break;
                }
            }

            return newPosition;
        }
    }
}