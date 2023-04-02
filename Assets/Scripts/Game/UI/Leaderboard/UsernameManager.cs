using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UniRx;
using UnityEngine;

namespace Game.UI.Leaderboard {
    public static class UsernameManager {
        
        public static async Task<string> TryGetUsername() {
            var result = await PlayFabLogin.TryLogin();
            if (!result) {
                return string.Empty;
            }

            var profileResultType = CommandResultType.Process;
            GetPlayerProfileResult profileResult = null;

            void OnProfileRequestComplete(GetPlayerProfileResult getPlayerProfileResult) {
                Debug.Log("OnProfileRequestComplete");
                profileResult = getPlayerProfileResult;
                profileResultType = CommandResultType.Success;
            }
            
            void OnProfileRequestError(PlayFabError playFabError) {
                profileResultType = CommandResultType.Fail;
                Debug.LogError(playFabError.GenerateErrorReport());
            }

            var profileRequest = new GetPlayerProfileRequest { PlayFabId = PlayFabLogin.LoginResult.PlayFabId };
            PlayFabClientAPI.GetPlayerProfile(profileRequest, OnProfileRequestComplete, OnProfileRequestError);
            
            bool IsComplete() => profileResultType != CommandResultType.Process;
            
            await Observable.EveryUpdate()
                .Where(_ => IsComplete())
                .FirstOrDefault()
                .ToTask();

            if (profileResultType == CommandResultType.Fail) {
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(profileResult?.PlayerProfile.DisplayName)) {
                return profileResult.PlayerProfile.DisplayName;
            }

            var userNameFromWindow = await EnterUserIdWindow.Instance.Show();
            if (string.IsNullOrEmpty(userNameFromWindow)) {
                return userNameFromWindow;
            }

            var displayNameRequest = new UpdateUserTitleDisplayNameRequest { DisplayName = userNameFromWindow };
            PlayFabClientAPI.UpdateUserTitleDisplayName(displayNameRequest, OnUpdateUserTitleDisplayName, OnUpdateUserTitleDisplayNameFailure);
            
            return userNameFromWindow;
        }

        private static void OnUpdateUserTitleDisplayName(UpdateUserTitleDisplayNameResult updateUserTitleDisplayNameResult) {
            Debug.Log("UpdateUserTitleDisplayNameSuccess");
        }
        
        private static void OnUpdateUserTitleDisplayNameFailure(PlayFabError error) {
            Debug.LogError(error.GenerateErrorReport());
        }
    }
}