using Game;
using Game.UI.Leaderboard;
using Tics;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class GameLoadedSignal {}

public class Main : MonoBehaviourSingleton<Main> {
    
    [SerializeField] private GameObject _loadingImage;
    [SerializeField] private AudioSource _gameMusic;
    [SerializeField] private AudioSource _menuMusic;
    
    public GameItemsManager Inventory { get; private set; }
    public ModificationsManager Modifications { get; private set; }

    private bool _isPaused;
    
    public static bool IsGameStarted  { get; private set; }


    private void Start() {
        LoadGame();
    }

    private void LoadGame() {
        Inventory = new GameItemsManager();
        Modifications = new ModificationsManager();
        _loadingImage.SetActive(true);
        var operation = SceneManager.LoadSceneAsync(sceneBuildIndex : 1, LoadSceneMode.Additive);
        operation.completed += OnSceneLoaded;
    }

    private void Update() {
        if (_isPaused) {
            return;
        }

        if (!IsGameStarted) {
            return;
        }
        
        TicMan.Update();
    }
    
    private void OnApplicationFocus(bool hasFocus) {
        
    }

    private void OnSceneLoaded(AsyncOperation asyncOperation) {
        _gameMusic.Play();
        _menuMusic.Stop();
        Observable.TimerFrame(30).Subscribe(_ => {
            IsGameStarted = true;
            _loadingImage.SetActive(false);
            MessageBroker.Default.Publish(new GameLoadedSignal());
        });
    }

    private void OnApplicationPause(bool isPaused) {
        _isPaused = isPaused;

        if (!IsGameStarted) {
            return;
        }
        
        // pause/resume game managers
    }

    public void Lose() {
        Debug.Log("LOSE");
        CompleteGameWithRestart();
    }

    public void Win() {
        Debug.Log("WIN");
        CalcExp();
        CompleteGameWithRestart();
    }

    private async void CompleteGameWithRestart() {
        IsGameStarted = false;
        _gameMusic.Stop();
        _menuMusic.Play();
        
        SceneManager.UnloadSceneAsync(1);
        await UserManager.TryGetUsername();
        await LeaderboardManager.AddScore(Inventory.Exp.Count);
        await LeaderboardWindow.Instance.Show();
        LoadGame();
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
