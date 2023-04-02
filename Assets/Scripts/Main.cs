using Game;
using Game.UI.Leaderboard;
using Tics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class Main : MonoBehaviourSingleton<Main> {
    public readonly GameItemsManager Inventory = new ();
    public readonly ModificationsManager Modifications = new ();
    
    private bool _isStarted;
    private bool _isPaused;
    
    private void Start() {
        _isStarted = true;
    }

    private void Update() {
        if (_isPaused) {
            return;
        }

        if (!_isStarted) {
            return;
        }
        
        TicMan.Update();
    }
    
    private void OnApplicationFocus(bool hasFocus) {
        
    }

    private void OnApplicationPause(bool isPaused) {
        _isPaused = isPaused;

        if (!_isStarted) {
            return;
        }
        
        // pause/resume game managers
    }

    public void Lose() {
        Debug.LogError("LOSE");
        CompleteGameWithRestart();
    }

    public void Win() {
        Debug.LogError("WIN");
        CalcExp();
        CompleteGameWithRestart();
    }

    private async void CompleteGameWithRestart() {
        Time.timeScale = 0;
        await UserManager.TryGetUsername();
        await LeaderboardManager.AddScore(Inventory.Coins.Count);
        await LeaderboardWindow.Instance.Show();
        Restart();
        Time.timeScale = 1;
    }

    private void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

#if UNITY_EDITOR
    private void OnEditorPauseStateChanged(PauseState state) {
        OnApplicationPause(state == PauseState.Paused);
    }
#endif

    private void CalcExp() {
        var exp = (int)CountdownTimer.Instance.timeRemaining + 10 * Inventory.Coins.Count;
        Inventory.Exp.SetCount(exp);
    }
}
