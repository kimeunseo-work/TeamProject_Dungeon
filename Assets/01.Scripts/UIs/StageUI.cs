using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private GameObject sellectSkillPanel;

    [Header("Panels")]
    [SerializeField] private Transform ContentAcquiredSkills;
    [SerializeField] private Transform ContentRandomSkills;

    //public SkillManager skillManager;

    private PlayerStatus playerStatus;

    private void Start()
    {
        settingsButton.onClick.AddListener(OpenSettingsPanel);
        SkillManager.Instance.Init(selectSkillPanel: ContentRandomSkills, acquiredSkillPanel: ContentAcquiredSkills);
        SceneManager.sceneLoaded += OnSceneChanged;
        //SkillManager.Instance.Init();
        //playerStatus.OnDungeonExpChanged += UpdateExp;
        //playerStatus.OnDungeonLevelChanged += UpdateLevel;
    }

    private void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("init");
        SkillManager.Instance.Init(selectSkillPanel: ContentRandomSkills, acquiredSkillPanel: ContentAcquiredSkills);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenSelectSkillPanel();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneChanged;
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
        OpenSelectSkillPanel();
    }

    private void UpdateExp()
    {
        expSlider.value = (float)playerStatus.DungeonExp / (float)playerStatus.RequiredDungeonExp;
    }
    #endregion

    #region Settings
    private void OpenSettingsPanel()
    {
        Time.timeScale = 0f;
        UIManager.Instance.PushUI(settingsPanel);
    }
    #endregion

    #region Select Skill
    private void OpenSelectSkillPanel()
    {
        //UIManager.Instance.PushUI(sellectSkillPanel);
        //SkillManager.Instance.ShowSkillLists();

        SkillManager.Instance.RequestOpenSkillPanel();
    }
    #endregion
}
