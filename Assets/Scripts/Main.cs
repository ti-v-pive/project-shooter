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
        if (hasFocus) {
            Cursor.visible = false;
        }
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
        CompleteGameWithRestart();
    }

    private async void CompleteGameWithRestart() {
        Time.timeScale = 0;
        await UsernameManager.TryGetUsername();
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
}
