using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageEndingUI : MonoBehaviour
{
    [SerializeField] private Button loadLobbyButton;
    [SerializeField] private TextMeshProUGUI endStageTitle;
    [SerializeField] private TextMeshProUGUI endStageText;

    private const string gameOverTitleStr = "Stage Failed";
    private const string gameOverStr = "Stage Failed...\nTry again hero!";

    private const string clearTitleStr = "Stage Clear";
    private const string clearStr = "Stage Cleared!\nWell done, hero!";

    private void Start()
    {
        loadLobbyButton.onClick.AddListener(OnClickLoadLobby);
    }

    public void Init(bool isClear)
    {
        if (isClear)
        {
            endStageTitle.text = gameOverTitleStr;
            endStageText.text = gameOverStr;
        }
        else
        {
            endStageTitle.text = clearTitleStr;
            endStageText.text = clearStr;
        }
    }

    private void OnClickLoadLobby()
    {
        GameManager.Instance.ChangeGameState(GameManager.GameState.LobbyScene);
    }
}
