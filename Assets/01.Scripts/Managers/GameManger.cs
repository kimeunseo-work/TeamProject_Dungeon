using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    public static GameManger Instance;

    /*필드 & 프로퍼티*/
    //=======================================// 

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

    /*외부 호출용*/
    //=======================================//

    /// <summary>
    /// 씬 로드도 이걸 사용하세요.
    /// </summary>
    /// <param name="state"></param>
    public void ChangeGameState(GameState state)
    {
        if (state == CurrentState) return;

        CurrentState = state;
        StartCoroutine(LoadScene());
    }

    /*내부 로직*/
    //=======================================//
    private IEnumerator LoadScene()
    {
        yield return UIManager.Instance.FadeOut();

        UIManager.Instance.PopUI();

        if(CurrentState == GameState.LobbyScene)
        {
            SceneManager.LoadScene(nameof(GameState.LobbyScene));
        }
        else if(CurrentState == GameState.DungeonScene)
        {
            SceneManager.LoadScene(nameof(GameState.DungeonScene));
        }

        yield return UIManager.Instance.FadeIn();
    }
}