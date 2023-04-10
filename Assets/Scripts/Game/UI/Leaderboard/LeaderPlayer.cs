using System.Collections;
using System.Linq;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
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
            SetFlagImage(entry.Profile?.Locations?.FirstOrDefault()?.CountryCode?.ToString());
        }

        public void Init(string nickname, int position, int coinsCount) {
            _number.text = position.ToString();
            _nickname.text = nickname;
            _score.text = coinsCount.ToString();
        }
        
        public void InitAsEmpty() {
            _number.text = "-";
            _nickname.text = "-";
            _score.text = "-";
        }

        private void SetFlagImage(string countryCode) {
            if (string.IsNullOrEmpty(countryCode)) {
                _flag.gameObject.SetActive(false);
                return;
            }
            string url = $"https://flagcdn.com/28x21/{countryCode.ToLower()}.png";
            StartCoroutine(DownloadFlagImage(url, SetImage));
        }

        private static IEnumerator DownloadFlagImage(string url, System.Action<Sprite> callback) {
            using UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success) {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                callback?.Invoke(sprite);
            } else {
                Debug.LogError($"Error downloading image: {request.error}");
            }
        }

        private void SetImage(Sprite sprite) {
            if (!_flag) {
                return;
            }
            _flag.sprite = sprite;
            _flag.SetNativeSize();
            _flag.gameObject.SetActive(true);
        }
    }
}