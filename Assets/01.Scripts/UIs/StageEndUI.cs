using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageEndUI : MonoBehaviour
{
    [SerializeField] private Button loadLobbyButton;

    private void Start()
    {
        loadLobbyButton.onClick.AddListener(OnClickLoadLobby);
    }

    private void OnClickLoadLobby()
    {
        GameManger.Instance.ChangeGameState(GameManger.GameState.LobbyScene);
    }
}
