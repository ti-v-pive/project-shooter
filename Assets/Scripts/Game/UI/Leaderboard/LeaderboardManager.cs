using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Game.UI.Leaderboard {
    public class LeaderboardManager {
        private const string PLAY_FAB_STATISTIC_NAME = "HighScore";

        public static void AddScore(int score) {
            var request = new UpdatePlayerStatisticsRequest {
                Statistics = new List<StatisticUpdate> {
                    new() { StatisticName = PLAY_FAB_STATISTIC_NAME, Value = score }
                }
            };

            PlayFabClientAPI.UpdatePlayerStatistics(request, OnScoreAdded, OnError);
        }

        public static void GetTopScores(int count, System.Action<List<PlayerLeaderboardEntry>> callback) {
            var request = new GetLeaderboardRequest {
                StatisticName = PLAY_FAB_STATISTIC_NAME,
                StartPosition = 0,
                MaxResultsCount = count
            };

            PlayFabClientAPI.GetLeaderboard(request, result => callback(result.Leaderboard), OnError);
        }

        private static void OnScoreAdded(UpdatePlayerStatisticsResult result) => Debug.Log("Score added successfully.");
        private static void OnError(PlayFabError error) => Debug.LogError($"PlayFab Error: {error.ErrorMessage}");
    }
}