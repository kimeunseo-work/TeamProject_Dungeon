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

    [Header("Panels Content Transform")]
    [SerializeField] private Transform ContentAcquiredSkills;
    [SerializeField] private Transform ContentRandomSkills;

    [SerializeField] private TextMeshProUGUI howGetSkillText;

    private PlayerStatus playerStatus;

    #region Life cycle
    private void Start()
    {
        settingsButton.onClick.AddListener(OpenSettingsPanel);
        SkillManager.Instance.Init(selectSkillPanel: ContentRandomSkills
                                        , acquiredSkillPanel: ContentAcquiredSkills
                                        , howGetSkillText: howGetSkillText);

        SceneManager.sceneLoaded += OnSceneLoaded;
        //playerStatus.OnDungeonExpChanged += UpdateExp;
        //playerStatus.OnDungeonLevelChanged += UpdateLevel;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenSelectSkillPanel("Stage Clear");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
        // 레벨, 골드, 경험치 세팅 이런식으로?
        //levelText.text = StatusManager.Instance.level.ToString();
        //goldText.text = StatusManager.Instance.gold.ToString();
        //expSlider.value = StatusManager.Instance.Exp / StatusManager.Instance.nextExp
    }

    private void UpdateLevel()
    {
        levelText.text = playerStatus.DungeonLevel.ToString();
        UpdateExp();
        OpenSelectSkillPanel("Level Up");
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
    private void OpenSelectSkillPanel(string howGetSkillStr)
    {
        SkillManager.Instance.RequestOpenSkillPanel(howGetSkillStr);
    }
    #endregion
}
