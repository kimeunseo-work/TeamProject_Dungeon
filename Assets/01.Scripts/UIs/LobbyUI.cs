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
        UpdateGoldUI();
        PlayerLobbyStatus.Instance.OnPointChanged += UpdateGoldUI;
        PlayerLobbyStatus.Instance.OnBaseExpChanged += UpdateExpUI;
        PlayerLobbyStatus.Instance.OnBaseLevelChanged += UpdateLevelUI;
    }

    private void OnDestroy()
    {
        if (PlayerLobbyStatus.Instance != null)
            PlayerLobbyStatus.Instance.OnPointChanged -= UpdateGoldUI;
    }

    private void UpdateGoldUI()
    {
        goldText.text = PlayerLobbyStatus.Instance.Point.ToString();
    }

    private void UpdateExpUI()
    {
        Debug.Log("playerExp" + PlayerLobbyStatus.Instance.BaseExp);
        float percentage = (float)(PlayerLobbyStatus.Instance.BaseExp) / 100;
        Debug.Log("percentage"+percentage);
        expFill.localScale = new Vector3(percentage, 1.0f, 1.0f);
        expPercentageText.text = $"{(int)(percentage * 100)}%";
    }

    private void UpdateLevelUI()
    {
        levelText.text = PlayerLobbyStatus.Instance.BaseLevel.ToString();
    }

    private void OnClickPlay()
    {
        UIManager.Instance.PopUI();
        GameManger.Instance.ChangeGameState(GameManger.GameState.DungeonScene);
    }
}
