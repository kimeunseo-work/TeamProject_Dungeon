using System.Collections.Generic;
using UnityEngine;
public class PlayerSkills : MonoBehaviour
{
    [SerializeField] public List<SkillData> acquiredSkills = new List<SkillData>();
    [SerializeField] private PlayerStatus playerStatus;

    private void Start()
    {
        SkillManager.Instance.Init(this);
    }

    public void AddSkill(SkillData skillData)
    {
        if (!skillData.canStack && acquiredSkills.Contains(skillData))
            return;

        acquiredSkills.Add(skillData);

        if (skillData.skillType == SkillType.Pasive)
            ApplyPassiveSkill(skillData);
    }

    private void ApplyPassiveSkill(SkillData skillData)
    {
        switch (skillData.statusType)
        {
            case StatusType.MaxHp:
                playerStatus.IncreaseDungeonHp(skillData.skillValue);
                break;
            case StatusType.Attack:
                playerStatus.IncreaseDungeonAtk(skillData.skillValue);
                break;
            default:
                break;
        }
    }
}
