using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI skillNameText;
    private SkillData skilldata;
    private Action<SkillData> onClickAction;

    public void Setup(SkillData skill, Action<SkillData> onClick)
    {
        skilldata = skill;
        onClickAction = onClick;
        iconImage.sprite = skill.icon;
        skillNameText.text = skill.name;
        GetComponent<Button>().onClick.AddListener(()=>onClickAction.Invoke(skilldata));
    }
}
