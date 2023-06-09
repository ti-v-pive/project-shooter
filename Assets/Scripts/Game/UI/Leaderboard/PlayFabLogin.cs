﻿using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UniRx;
using UnityEngine;

namespace Game.UI.Leaderboard {
    public static class PlayFabLogin {

        public static LoginResult LoginResult;

        public static async Task<bool> TryLogin() {
            NetLoadingImage.SetActive(true);
            if (PlayFabClientAPI.IsClientLoggedIn()) {
                return true;
            }
            
            var resultType = CommandResultType.Process;

            if (!PlayerPrefs.HasKey("uniqueID")) {
                PlayerPrefs.SetString("uniqueID", System.Guid.NewGuid().ToString());
                PlayerPrefs.Save();
            }

            string uniqueID = PlayerPrefs.GetString("uniqueID");
            
            var loginRequest = new LoginWithCustomIDRequest { CustomId = uniqueID, CreateAccount = true };
            
            void OnLoginSuccess(LoginResult result) {
                LoginResult = result;
                resultType = CommandResultType.Success;
                Debug.Log("OnLoginSuccess");
            }
            
            void OnLoginFailure(PlayFabError error) {
                Debug.LogError(error.GenerateErrorReport());
                resultType = CommandResultType.Fail;
            }

            PlayFabClientAPI.LoginWithCustomID(loginRequest, OnLoginSuccess, OnLoginFailure);

            bool IsComplete() => resultType != CommandResultType.Process;
            
            await Observable.EveryUpdate()
                .Where(_ => IsComplete())
                .FirstOrDefault()
                .ToTask();

            NetLoadingImage.SetActive(false);
            return resultType == CommandResultType.Success;
        }
    }
}