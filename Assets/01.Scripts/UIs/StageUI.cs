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
    [SerializeField] private TextMeshProUGUI currentStageText;

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject sellectSkillPanel;
    [SerializeField] private GameObject StageEndingPanel;

    [Header("Panels Content Transform")]
    [SerializeField] private Transform ContentAcquiredSkills;
    [SerializeField] private Transform ContentRandomSkills;

    [SerializeField] private TextMeshProUGUI howGetSkillText;
    [SerializeField] private StageEndingUI stageEndingUI;

    private PlayerStatus playerStatus;

    #region Life cycle
    private void Start()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();

        SkillManager.Instance.Init(selectSkillPanel: ContentRandomSkills
                                        , acquiredSkillPanel: ContentAcquiredSkills
                                        , howGetSkillText: howGetSkillText);

        settingsButton.onClick.AddListener(OpenSettingsPanel);

        SceneManager.sceneLoaded += OnSceneLoaded;
        playerStatus.OnDungeonLevelChanged += OnLevelUp;
        playerStatus.OnDungeonExpChanged += UpdateExp;
        playerStatus.OnDead += OpenGameOverUI;

        StageManager.Instance.OnAllStageCleared += OpenStageClearUI;
        StageManager.Instance.OnStageChanged += UpdateCurrentStage;

        UpdateHUD();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        playerStatus.OnDungeonLevelChanged -= OnLevelUp;
        playerStatus.OnDungeonExpChanged -= UpdateExp;
        playerStatus.OnDead -= OpenGameOverUI;
        StageManager.Instance.OnAllStageCleared -= OpenStageClearUI;
    }
    #endregion

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SkillManager.Instance.Init(selectSkillPanel: ContentRandomSkills
                                    , acquiredSkillPanel: ContentAcquiredSkills
                                    , howGetSkillText: howGetSkillText);
    }

    #region Game
    private void UpdateHUD()
    {
        UpdateLevel();
        UpdateExp();
    }

    private void UpdateLevel()
    {
        levelText.text = playerStatus.DungeonLevel.ToString();
    }

    private void UpdateExp()
    {
        float targetValue;
        if (playerStatus.RequiredDungeonExp == 0)
            targetValue = (float)playerStatus.DungeonExp / 1;
        else
            targetValue = (float)playerStatus.DungeonExp / playerStatus.RequiredDungeonExp;

        //expSlider.value = targetValue;
        UIManager.Instance.AnimateSlider(expSlider, targetValue);
    }

    private void OnLevelUp()
    {
        Debug.Log("level up");
        UIManager.Instance.ResetSlider(expSlider);
        UpdateLevel();
        OpenSelectSkillPanel("Level Up");
        UpdateExp();
    }

    private void UpdateCurrentStage(int currentStage)
    {
        currentStageText.text = currentStage.ToString();
    }
    #endregion

    #region Settings
    private void OpenSettingsPanel()
    {
        AudioManager.instance.PlayButtonClick();
        Time.timeScale = 0f;
        UIManager.Instance.PushUI(settingsPanel);
    }
    #endregion
    
    #region Select Skill
    private void OpenSelectSkillPanel(string howGetSkillStr)
    {
        SkillManager.Instance.RequestOpenSkillPanel(howGetSkillStr);
    }
    #endregion

    #region Stage Ending UI
    private void OpenGameOverUI()
    {
        Time.timeScale = 0f;
        stageEndingUI.Init(false);
        UIManager.Instance.PushUI(StageEndingPanel);
    }

    public void OpenStageClearUI()
    {
        Time.timeScale = 0f;
        stageEndingUI.Init(true);
        UIManager.Instance.PushUI(StageEndingPanel);
    }
    #endregion
}
