using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour 
{
    public List<SkillData> allSkills;

    public GameObject skillButtonPrefab;
    public Transform selectSkillPanel;

    public GameObject acquiredSkillPrefab;
    public Transform acquiredSkillPanel;

    public void ShowSkillLists()
    {
        UIManager.Instance.PushUI(selectSkillPanel.parent.gameObject);
        //selectSkillPanel.gameObject.SetActive(true);

        List<SkillData> options = allSkills
            .Where(
            s => s.canStack 
            || !PlayerSkills.Instance.acquiredSkills.Contains(s)
            )
            .OrderBy(x => Random.value)
            .Take(4)
            .ToList();

        foreach (Transform child in selectSkillPanel)
            Destroy(child.gameObject);

        foreach(var skill in options)
        {
            GameObject btnObj = Instantiate(skillButtonPrefab, selectSkillPanel);
            btnObj.GetComponent<SkillButton>().Setup(skill, OnSkillSelected);
        }
    }

    void OnSkillSelected(SkillData skill)
    {
        PlayerSkills.Instance.AddSkill(skill);
        UIManager.Instance.PopUI();
        //selectSkillPanel.gameObject.SetActive(false);

        GameObject imgObj = Instantiate(acquiredSkillPrefab, acquiredSkillPanel);
        imgObj.GetComponent<SkillIcon>().SetUp(skill.icon);
    }
}
