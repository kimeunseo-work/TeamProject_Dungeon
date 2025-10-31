using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [Header("Level & Exp")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private RectTransform expFill;
    [SerializeField] private TextMeshProUGUI expPercentageText;

    [Header("Gold")]
    [SerializeField] private TextMeshProUGUI goldText;

    [Header("Play")]
    [SerializeField] private Button playButton;

    private PlayerStatus playerStatus;

    private void Awake()
    {
        playButton.onClick.AddListener(OnClickPlay);
    }

    private void Start()
    {
        UIManager.Instance.PushUI(gameObject);
        AllUIs();
        PlayerLobbyStatus.Instance.OnPointChanged += UpdateGoldUI;
        PlayerLobbyStatus.Instance.OnBaseExpChanged += UpdateExpUI;
        PlayerLobbyStatus.Instance.OnBaseLevelChanged += UpdateLevelUI;
    }

    private void OnDestroy()
    {
        if (PlayerLobbyStatus.Instance != null)
        {
            PlayerLobbyStatus.Instance.OnPointChanged -= UpdateGoldUI;
            PlayerLobbyStatus.Instance.OnBaseExpChanged -= UpdateExpUI;
            PlayerLobbyStatus.Instance.OnBaseLevelChanged -= UpdateLevelUI;
        }
    }

    private void AllUIs()
    {
        UpdateGoldUI();
        UpdateExpUI();
        UpdateLevelUI();
    }

    private void UpdateGoldUI()
    {
        goldText.text = PlayerLobbyStatus.Instance.Point.ToString();
    }

    private void UpdateExpUI()
    {
        float percentage = (float)(PlayerLobbyStatus.Instance.BaseExp) / PlayerLobbyStatus.Instance.RequiredBaseExp;
        expFill.localScale = new Vector3(percentage, 1.0f, 1.0f);
        int intPercentage = (int)(percentage * 100);
        expPercentageText.text = $"{intPercentage}%";
    }

    private void UpdateLevelUI()
    {
        levelText.text = PlayerLobbyStatus.Instance.BaseLevel.ToString();
        UpdateExpUI();
    }

    private void OnClickPlay()
    {
        UIManager.Instance.PopUI();
        GameManger.Instance.ChangeGameState(GameManger.GameState.DungeonScene);
    }
}
