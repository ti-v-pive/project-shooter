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

        [SerializeField] private List<LeaderPlayer> _players;
        [SerializeField] private LeaderPlayer _myStat;
        [SerializeField] private Button _buttonClose;

        private bool _closeButtonClicked;
        
        protected override void Awake() {
            base.Awake();
            
            if (_buttonClose) {
                _buttonClose.onClick.AddListener(OnCloseClick);
            }
            
            HideInternal();
        }

        public async Task Show() {
            _closeButtonClicked = false;
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
            
            var nickname = await UserManager.TryGetUsername(false);
            if (!string.IsNullOrEmpty(nickname)) {
                var score = Main.Instance.Inventory.Exp.Count;
                var position = CalculatePositionForNewScore(scores, score);
                _myStat.Init(nickname, position, score);
            } else {
                _myStat.InitAsEmpty();
            }
            
            await Observable.EveryUpdate()
                .Where(_ => _closeButtonClicked)
                .FirstOrDefault()
                .ToTask();
            
            HideInternal();
        }

        private void ShowInternal() => gameObject.SetActive(true);
        private void HideInternal() => gameObject.SetActive(false);

        private void OnCloseClick() => _closeButtonClicked = true;

        private static int CalculatePositionForNewScore(List<PlayerLeaderboardEntry> entries, int newScore) {
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