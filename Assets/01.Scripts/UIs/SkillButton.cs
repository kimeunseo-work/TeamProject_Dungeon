using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI skillDescriptionText;
    [SerializeField] private RectTransform slotContainer;

    private Button button;
    private LayoutElement layoutElement;

    private SkillData skillData;
    private Action<SkillData> onClickAction;

    private void Awake()
    {
        button = GetComponent<Button>();

        layoutElement = GetComponent<LayoutElement>();
        if (layoutElement != null)
            layoutElement.ignoreLayout = true;

        slotContainer = GetComponent<RectTransform>();
    }

    public void Setup(SkillData skill, Action<SkillData> onClick)
    {
        button.onClick.RemoveAllListeners();

        skillData = skill;
        onClickAction = onClick;

        if (iconImage != null)
            iconImage.sprite = skill.icon;

        if (skillNameText != null)
            skillNameText.text = skill.name;

        if (skillDescriptionText != null)
            skillDescriptionText.text = skill.skillDescription;

        if (onClickAction != null)
            button.onClick.AddListener(() => onClickAction.Invoke(skillData));
    }

    public SkillData GetSkill() => skillData;

    public void PlayDropAnimation(float distance = 50f, float duration = 0.5f)
    {
        StartCoroutine(DropAnimation(distance, duration));
    }

    private IEnumerator DropAnimation(float distance, float duration)
    {
        Vector3 startPos = slotContainer.localPosition + Vector3.up * distance;
        Vector3 endPos = slotContainer.localPosition;
        slotContainer.localPosition = startPos;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            slotContainer.localPosition = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        slotContainer.localPosition = endPos;
    }
}
