using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [Header("Slot Machine Effect")]
    private float spinDuration = 1.5f;
    private float delayBetweenStop = 0.4f;
    private float spinSpeed = 0.05f;
    private List<SlotButton> slotButtons = new();

    WaitForSecondsRealtime waitForSecondsRealtimeToSpinSpeed;
    WaitForSecondsRealtime waitForSecondsRealtimeToDelayBetweenStop;

    private class SlotButton
    {
        public SkillButton skillButton;
        public Button button;
    }

    #region Life Cycle
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        waitForSecondsRealtimeToSpinSpeed = new WaitForSecondsRealtime(spinSpeed);
        waitForSecondsRealtimeToDelayBetweenStop = new WaitForSecondsRealtime(delayBetweenStop);

    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    #endregion

    private void OnSceneUnloaded(Scene scene)
    {
        StopAllCoroutines();
        slotButtons.Clear();
    }

    public void Init(PlayerSkills playerSkills) => this.playerSkills = playerSkills;

    public void Init(Transform selectSkillPanel, Transform acquiredSkillPanel, TextMeshProUGUI howGetSkillText)
    {
        this.selectSkillPanel = selectSkillPanel;
        this.acquiredSkillPanel = acquiredSkillPanel;
        this.howGetSkillText = howGetSkillText;
    }

    #region Selecting Skill
    public void RequestOpenSkillPanel(string howGetSkillStr, Action onComplete = null)
    {
        Time.timeScale = 0f;

        if (isSelectingSkill)
        {
            //skillSelectQueue.Enqueue(() => OpenSelectSkillPanel(howGetSkillStr, onComplete));
            skillSelectQueue.Enqueue(() => StartCoroutine(SlotMachineEffect(howGetSkillStr, onComplete)));
            return;
        }

        //OpenSelectSkillPanel(howGetSkillStr, onComplete);
        StartCoroutine(SlotMachineEffect(howGetSkillStr, onComplete));
    }

    // no animation
    //public void OpenSelectSkillPanel(string howGetSkillStr, Action onComplete)
    //{
    //    isSelectingSkill = true;
    //    UIManager.Instance.PushUI(selectSkillPanel.parent.gameObject);

    //    List<SkillData> options = allSkills
    //        .Where(
    //        s => s.canStack
    //        || !playerSkills.acquiredSkills.Contains(s)
    //        )
    //        .OrderBy(x => UnityEngine.Random.value)
    //        .Take(3)
    //        .ToList();

    //    foreach (Transform child in selectSkillPanel)
    //        Destroy(child.gameObject);

    //    this.howGetSkillText.text = howGetSkillStr;

    //    // slot machine effect

    //    foreach (var skill in options)
    //    {
    //        Debug.Log(skill.name);
    //        GameObject btnObj = Instantiate(skillButtonPrefab, selectSkillPanel);
    //        btnObj.GetComponent<SkillButton>().Setup(skill, OnSkillSelected);
    //    }

    //    void OnSkillSelected(SkillData seleted)
    //    {
    //        // ������ ���
    //        playerSkills.AddSkill(seleted);

    //        GameObject imgObj = Instantiate(acquiredSkillPrefab, acquiredSkillPanel);
    //        imgObj.GetComponent<SkillIcon>().SetUp(seleted.icon);

    //        CloseSkillPanel();
    //        onComplete?.Invoke();
    //    }
    //}

    private IEnumerator SlotMachineEffect(string howGetSkillStr, Action onComplete)
    {
        if (selectSkillPanel == null || selectSkillPanel.Equals(null) ||
    selectSkillPanel.parent == null || selectSkillPanel.parent.Equals(null))
        {
            yield break;
        }

        isSelectingSkill = true;
        howGetSkillText.text = howGetSkillStr;
        UIManager.Instance.PushUI(selectSkillPanel.parent.gameObject);

        foreach (Transform child in selectSkillPanel)
            Destroy(child.gameObject);

        slotButtons.Clear();

        for (int i = 0; i < 3; i++)
        {
            GameObject btn = Instantiate(skillButtonPrefab, selectSkillPanel);
            var skillButton = btn.GetComponent<SkillButton>();
            var button = btn.GetComponent<Button>();

            button.interactable = false;
            slotButtons.Add(new SlotButton { skillButton = skillButton, button = button });
        }

        // slot spin
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < spinDuration)
        {
            if (selectSkillPanel == null || !selectSkillPanel.gameObject.activeInHierarchy)
            {
                Debug.Log("skill panel closed");
                yield break;
            }

            foreach (var slot in slotButtons)
            {
                var randomSkill = allSkills[UnityEngine.Random.Range(0, allSkills.Count)];
                slot.skillButton.Setup(randomSkill, null);
            }

            yield return waitForSecondsRealtimeToSpinSpeed;
        }

        var finalOptions = allSkills
            .Where(s => s.CanStack || !playerSkills.AcquiredSkills.Contains(s))
            .OrderBy(x => UnityEngine.Random.value)
            .Take(3)
            .ToList();

        // stop spin
        for (int i = 0; i < slotButtons.Count; i++)
        {
            if (selectSkillPanel == null || !selectSkillPanel.gameObject.activeInHierarchy)
            {
                Debug.Log("skill panel closed");
                yield break;
            }

            yield return waitForSecondsRealtimeToDelayBetweenStop;

            var slot = slotButtons[i];
            var skill = finalOptions[i];// OUT OF RANGE

            slot.skillButton.Setup(skill, OnSkillSelected);
            slot.button.interactable = true;
            slot.skillButton.PlayDropAnimation();
        }

        void OnSkillSelected(SkillData seleted)
        {
            // ������ ���
            playerSkills.AddSkill(seleted);

            GameObject imgObj = Instantiate(acquiredSkillPrefab, acquiredSkillPanel);
            imgObj.GetComponent<SkillIcon>().SetUp(seleted.Icon);

            CloseSkillPanel();
            onComplete?.Invoke();
        }
    }

    private void CloseSkillPanel()
    {
        StopAllCoroutines();
        UIManager.Instance.PopUI();
        isSelectingSkill = false;

        if (skillSelectQueue.Count > 0)
        {
            var next = skillSelectQueue.Dequeue();
            next?.Invoke();
        }

        Time.timeScale = 1f;
    }
    #endregion
}