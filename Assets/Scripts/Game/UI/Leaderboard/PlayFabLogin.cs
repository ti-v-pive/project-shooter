using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UniRx;
using UnityEngine;

namespace Game.UI.Leaderboard {
    public static class PlayFabLogin {
        
        private enum LoginResultType {
            Process,
            Success,
            Fail
        }

        public static string PlayFabId;

        public static async Task<bool> TryLogin() {
            var resultType = LoginResultType.Process;
            var loginRequest = new LoginWithCustomIDRequest { CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true };
            
            void OnLoginSuccess(LoginResult result) {
                PlayFabId = result.PlayFabId;
                Debug.Log("OnLoginSuccess");
                resultType = LoginResultType.Success;
            }
            
            void OnLoginFailure(PlayFabError error) {
                Debug.LogError(error.GenerateErrorReport());
                resultType = LoginResultType.Fail;
            }

            PlayFabClientAPI.LoginWithCustomID(loginRequest, OnLoginSuccess, OnLoginFailure);

            bool IsComplete() => resultType != LoginResultType.Process;
            //await Observable.TakeWhile(IsComplete);
            return true;
        }
    }
}