using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private Button settingsButton;

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject levelUpPanel;


    private PlayerStatus playerStatus;

    private void Start()
    {
        settingsButton.onClick.AddListener(OpenSettingsPanel);
        playerStatus.OnDungeonExpChanged += UpdateExp;
        playerStatus.OnDungeonLevelChanged += UpdateLevel;
    }

    #region Game UI
    private void UpdateHUD()
    {
        // 레벨, 골드, 경험치 세팅 이런식으로?
        //levelText.text = StatusManager.Instance.level.ToString();
        //goldText.text = StatusManager.Instance.gold.ToString();
        //expSlider.value = StatusManager.Instance.Exp / StatusManager.Instance.nextExp
    }

    private void UpdateLevel()
    {
        levelText.text = playerStatus.DungeonLevel.ToString();
        UpdateExp();
        OpenLevelUpPanel();
    }

    private void UpdateExp()
    {
        //expSlider.value = (float)playerStatus.BaseExp / (float)playerStatus.RequiredBaseExp;
    }
    #endregion

    #region Settings
    private void OpenSettingsPanel()
    {
        Time.timeScale = 0f;
        UIManager.Instance.PushUI(settingsPanel);
    }
    #endregion

    #region Level Up
    private void OpenLevelUpPanel()
    {
        UIManager.Instance.PushUI(levelUpPanel);
    }
    #endregion
}
