using System.Collections.Generic;
using UnityEngine;
public class PlayerSkills : MonoBehaviour
{
    public static PlayerSkills Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<SkillData> acquiredSkills = new List<SkillData>();

    public void AddSkill(SkillData skill)
    {
        if (!skill.canStack && acquiredSkills.Contains(skill))
            return;

        acquiredSkills.Add(skill);

        if (skill.type == SkillType.Pasive)
            ApplyPassiveSkill(skill);
    }

    private void ApplyPassiveSkill(SkillData skill)
    {
        switch(skill.skillName)
        {
            case "Increase Attack":
                // increase dungeon attack
                PlayerLobbyStatus.Instance.IncreaseBaseAtk();
                break;
            case "Increase Hp":
                // increase dungeon hp
                PlayerLobbyStatus.Instance.IncreaseBaseHp();
                break;
            case "Increase Attack Speed":
                // increase dungeon attack Speed
                break;
        }
    }
}
