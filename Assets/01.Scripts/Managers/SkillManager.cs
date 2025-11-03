using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    [SerializeField] private TextMeshProUGUI howGetSkillText;

    [Header("All Skills Data")]
    [SerializeField] private List<SkillData> allSkills;

    [Header("Select Skill UI")]
    [SerializeField] private GameObject skillButtonPrefab;
    private Transform selectSkillPanel;

    [Header("Acquired Skill UI")]
    [SerializeField] private GameObject acquiredSkillPrefab;
    private Transform acquiredSkillPanel;

    private bool isSelectingSkill = false;
    private Queue<Action> skillSelectQueue = new Queue<Action>();

    private PlayerSkills playerSkills;

    #region Life Cycle
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    public void Init(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;
    }

    public void Init(Transform selectSkillPanel, Transform acquiredSkillPanel, TextMeshProUGUI howGetSkillText)
    {
        this.selectSkillPanel = selectSkillPanel;
        this.acquiredSkillPanel = acquiredSkillPanel;
        this.howGetSkillText = howGetSkillText;
    }

    public void RequestOpenSkillPanel(string howGetSkillStr, Action onComplete = null)
    {
        Time.timeScale = 0f;

        if (isSelectingSkill)
        {
            skillSelectQueue.Enqueue(() => OpenSelectSkillPanel(howGetSkillStr, onComplete));
            return;
        }

        OpenSelectSkillPanel(howGetSkillStr, onComplete);
    }

    public void OpenSelectSkillPanel(string howGetSkillStr, Action onComplete)
    {
        isSelectingSkill = true;
        UIManager.Instance.PushUI(selectSkillPanel.parent.gameObject);

        List<SkillData> options = allSkills
            .Where(
            s => s.CanStack
            || !playerSkills.AcquiredSkills.Contains(s)
            )
            .OrderBy(x => UnityEngine.Random.value)
            .Take(3)
            .ToList();

        foreach (Transform child in selectSkillPanel)
            Destroy(child.gameObject);

        this.howGetSkillText.text = howGetSkillStr;

        foreach (var skill in options)
        {
            //Debug.Log(skill.name);
            GameObject btnObj = Instantiate(skillButtonPrefab, selectSkillPanel);
            btnObj.GetComponent<SkillButton>().Setup(skill, OnSkillSelected);
        }

        void OnSkillSelected(SkillData selected)
        {
            playerSkills.AddSkill(selected);

            GameObject imgObj = Instantiate(acquiredSkillPrefab, acquiredSkillPanel);
            imgObj.GetComponent<SkillIcon>().SetUp(selected.Icon);

            CloseSkillPanel();
            onComplete?.Invoke();
        }
    }

    private void CloseSkillPanel()
    {
        UIManager.Instance.PopUI();
        isSelectingSkill = false;

        if (skillSelectQueue.Count > 0)
        {
            var next = skillSelectQueue.Dequeue();
            next?.Invoke();
        }

        Time.timeScale = 1f;
    }

    public SkillData GetInitSkill() => allSkills[0];
}