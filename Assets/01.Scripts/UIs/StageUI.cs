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

    private void Start()
    {
        settingsButton.onClick.AddListener(OpenSettingsPanel);
    }

    private void UpdateHUD()
    {
        // 레벨, 골드, 경험치 세팅 이런식으로?
        //levelText.text = StatusManager.Instance.level.ToString();
        //goldText.text = StatusManager.Instance.gold.ToString();
        //expSlider.value = StatusManager.Instance.Exp / StatusManager.Instance.nextExp
    }

    private void OpenSettingsPanel()
    {
        UIManager.Instance.PushUI(settingsPanel);
    }

    private void OpenLevelUpPanel()
    {
        UIManager.Instance.PushUI(levelUpPanel);
    }
}
