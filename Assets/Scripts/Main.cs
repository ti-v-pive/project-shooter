using Game;
using Tics;
using UnityEditor;
using UnityEngine;
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

#if UNITY_EDITOR
    private void OnEditorPauseStateChanged(PauseState state) {
        OnApplicationPause(state == PauseState.Paused);
    }
#endif
}
