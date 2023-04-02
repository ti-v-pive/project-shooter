using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Game.UI.Leaderboard {
    public static class UsernameManager {
        
        public static async Task<bool> TryGetUsername() {
            if (!PlayFabClientAPI.IsClientLoggedIn()) {
                var result = await PlayFabLogin.TryLogin();
                if (!result) {
                    return false;
                }
            }

            //var displayNameRequest = new GetPlayerProfileRequest { PlayFabId = SystemInfo.deviceUniqueIdentifier } ;
            //if(PlayFabClientAPI.GetPlayerProfile())
            
            //var displayNameRequest = new UpdateUserTitleDisplayNameRequest { DisplayName = _userId };
            //PlayFabClientAPI.UpdateUserTitleDisplayName(displayNameRequest, OnUpdateUserTitleDisplayName, OnUpdateUserTitleDisplayNameFailure);
            
            return true;
        }
        
        
        private static void OnUpdateUserTitleDisplayName(UpdateUserTitleDisplayNameResult updateUserTitleDisplayNameResult) {
            Debug.Log("UpdateUserTitleDisplayNameSuccess");
        }
        
        private static void OnUpdateUserTitleDisplayNameFailure(PlayFabError error) {
            Debug.LogError(error.GenerateErrorReport());
        }
    }
}