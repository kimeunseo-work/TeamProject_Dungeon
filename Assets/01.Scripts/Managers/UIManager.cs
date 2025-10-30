using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private Stack<GameObject> uiStack = new Stack<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerLobbyStatus.Instance.IncreasePoint();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayerLobbyStatus.Instance.IncreaseBaseExp(10);
        }
    }

    public void PushUI(GameObject ui)
    {
        //if (uiStack.Count > 0)
        //    uiStack.Peek().SetActive(false);

        ui.SetActive(true);
        uiStack.Push(ui);
    }

    public void PopUI()
    {
        if (uiStack.Count == 0)
        {
            Debug.Log("uiStack is empty");
            return;
        }

        GameObject topUI = uiStack.Pop();
        topUI.SetActive(false);

        if (uiStack.Count > 0)
            uiStack.Peek().SetActive(true);
    }

    public void ClearStack()
    {
        while (uiStack.Count > 0)
            uiStack.Pop().SetActive(false);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case nameof(GameManger.GameState.LobbyScene):
                Debug.Log("load lobby scene");
                LobbyUI lobbyUI = FindObjectOfType<LobbyUI>();

                if (lobbyUI != null)
                    uiStack.Push(lobbyUI.gameObject);

                break;
            case nameof(GameManger.GameState.DungeonScene):
                Debug.Log("load dungeon scene");
                StageUI stageUI = FindObjectOfType<StageUI>();

                if (stageUI != null)
                    uiStack.Push(stageUI.gameObject);

                break;
        }
    }
}
