using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UniRx;
using UnityEngine;

namespace Game.UI.Leaderboard {
    public static class UserManager {
        
        public static async Task<string> TryGetUsername() {
            NetLoadingImage.SetActive(true);
            var result = await PlayFabLogin.TryLogin();
            if (!result) {
                return string.Empty;
            }
            
            var playerProfile = await TryGetUserProfile();

            if (!string.IsNullOrEmpty(playerProfile.DisplayName)) {
                return playerProfile.DisplayName;
            }

            var userNameFromWindow = await EnterUserIdWindow.Instance.Show();
            if (string.IsNullOrEmpty(userNameFromWindow)) {
                return userNameFromWindow;
            }

            var displayNameRequest = new UpdateUserTitleDisplayNameRequest { DisplayName = userNameFromWindow };
            PlayFabClientAPI.UpdateUserTitleDisplayName(displayNameRequest, OnUpdateUserTitleDisplayName, OnUpdateUserTitleDisplayNameFailure);
            NetLoadingImage.SetActive(false);
            return userNameFromWindow;
        }

        public static async Task<PlayerProfileModel> TryGetUserProfile() {
            NetLoadingImage.SetActive(true);
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

            NetLoadingImage.SetActive(false);
            return profileResultType == CommandResultType.Fail ? null : profileResult.PlayerProfile;
        }

        private static void OnUpdateUserTitleDisplayName(UpdateUserTitleDisplayNameResult updateUserTitleDisplayNameResult) {
            Debug.Log("UpdateUserTitleDisplayNameSuccess");
        }
        
        private static void OnUpdateUserTitleDisplayNameFailure(PlayFabError error) {
            Debug.LogError(error.GenerateErrorReport());
        }
    }
}