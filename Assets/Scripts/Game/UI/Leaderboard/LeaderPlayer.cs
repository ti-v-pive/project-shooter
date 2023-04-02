using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Leaderboard {
    public class LeaderPlayer : MonoBehaviour {
        [SerializeField] private TMP_Text _number;
        [SerializeField] private TMP_Text _nickname;
        [SerializeField] private Image _flag;
        [SerializeField] private TMP_Text _score;

        public void Init(PlayerLeaderboardEntry entry) {
            if (entry == null) {
                return;
            }
            _number.text = entry.Position.ToString();
            _nickname.text = entry.DisplayName;
            _score.text = entry.StatValue.ToString();
        }
        
        public void Init(string nickname, int coinsCount) {
            _number.text = "-";
            _nickname.text = nickname;
            _score.text = coinsCount.ToString();
        }
    }
}