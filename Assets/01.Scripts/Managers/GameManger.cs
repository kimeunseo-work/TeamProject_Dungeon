using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    public static GameManger Instance;

    public enum GameState { LobbyScene, DungeonScene }
    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ChangeGameState(GameState state)
    {
        if (state == CurrentState) return;

        CurrentState = state;
        LoadScene();
    }

    private void LoadScene()
    {
        if(CurrentState == GameState.LobbyScene)
        {
            SceneManager.LoadScene(nameof(GameState.LobbyScene));
        }
        else if(CurrentState == GameState.DungeonScene)
        {
            SceneManager.LoadScene(nameof(GameState.DungeonScene));
        }
    }
}