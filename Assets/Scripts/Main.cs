using Tics;
using UnityEditor;
using UnityEngine;

public class Main : MonoBehaviour {
    private bool _isStarted;
    private bool _isPaused;
    
    private void Start() {
        _isStarted = true;
        
        // TODO: remove it
        /*
        // пример работы с твинами
        // берем твин из кэша или создаем новый
        var tm = TicMan.Create(()=> { Debug.Log("All tweens inside tm completed"); }); 

        tm.DoAfter(1, () => Debug.Log("After 1 sec first"));
        tm.DoAfter(1, () => Debug.Log("After 1 sec second")); // гарантирует выполнение после предыдущего вызова с таким же duration
        tm.DoAfter(2, () => Debug.Log("After 2 sec"));
        
        // через 0.5 сек начать двигать бесконечно туда-сюда трансформ с ease
        tm.MoveLocal(transform, Vector2.one, 1.5f)
            .SetDelay(0.5f)
            .SetEase(EaseType.BackInOut)
            .SetRepeat(-1, RepeatType.Yoyo);

        // через 2 сек остановит и зачистит tm, его можно начать переиспользовать
        tm.DoAfter(2f, tm.Reset);
        
        // через 1.5 сек убьет tm (остановит все свои действия, попадет в кэш, ссылку после kill нужно затереть)
        tm.DoAfter(1.5f, tm.Kill);
        */
    }

    private void Update() {
        if (_isPaused) {
            return;
        }
        
        TicMan.Update();
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
