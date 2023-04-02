using System.Collections.Generic;
using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UniRx;
using UnityEngine;

namespace Game.UI.Leaderboard {
    public class LeaderboardManager {
        private const string PLAY_FAB_STATISTIC_NAME = "HighScore";

        public static async void AddScore(int score) {
            var result = await PlayFabLogin.TryLogin();
            if (!result) {
                return;
            }
            
            var resultType = CommandResultType.Process;
            
            void OnScoreAdded(UpdatePlayerStatisticsResult result) {
                Debug.Log("Score added successfully.");
                resultType = CommandResultType.Success;
            }

            void OnError(PlayFabError error) {
                Debug.LogError($"PlayFab Error: {error.ErrorMessage}");
                resultType = CommandResultType.Fail;
            }

            var request = new UpdatePlayerStatisticsRequest {
                Statistics = new List<StatisticUpdate> {
                    new() { StatisticName = PLAY_FAB_STATISTIC_NAME, Value = score }
                }
            };

            PlayFabClientAPI.UpdatePlayerStatistics(request, OnScoreAdded, OnError);
            
            bool IsComplete() => resultType != CommandResultType.Process;
            
            await Observable.EveryUpdate()
                .Where(_ => IsComplete())
                .FirstOrDefault()
                .ToTask();
        }

        public static async Task<List<PlayerLeaderboardEntry>> GetTopScores(int count) {
            var loginResult = await PlayFabLogin.TryLogin();
            if (!loginResult) {
                return null;
            }
            
            var request = new GetLeaderboardRequest {
                StatisticName = PLAY_FAB_STATISTIC_NAME,
                StartPosition = 0,
                MaxResultsCount = count
            };
            
            var resultType = CommandResultType.Process;
            GetLeaderboardResult result = null;
            
            void ResultCallback(GetLeaderboardResult getLeaderboardResult) {
                Debug.Log("Score added successfully.");
                resultType = CommandResultType.Success;
                result = getLeaderboardResult;
            }

            void ErrorCallback(PlayFabError error) {
                Debug.LogError($"PlayFab Error: {error.ErrorMessage}");
                resultType = CommandResultType.Fail;
            }

            PlayFabClientAPI.GetLeaderboard(request, ResultCallback, ErrorCallback);
            
            bool IsComplete() => resultType != CommandResultType.Process;
            
            await Observable.EveryUpdate()
                .Where(_ => IsComplete())
                .FirstOrDefault()
                .ToTask();
            
            return result.Leaderboard;
        }
    }
}