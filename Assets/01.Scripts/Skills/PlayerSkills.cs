using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    public static PlayerSkills Instance;

    [SerializeField] public List<SkillData> AcquiredSkills = new List<SkillData>(10);
    
    [SerializeField] private PlayerStatus playerStatus;
    private BaseSkill defaultSkill;

    private void Reset()
    {
        playerStatus = GetComponent<PlayerStatus>();
        defaultSkill = GetComponentInChildren<BaseSkill>();
    }

    private void Start()
    {
        SkillManager.Instance.Init(this);

        if (defaultSkill == null)
        {
            Debug.Log("[PlayerSkills] defaultSkill is NULL, Attached script <BaseSkill>");
            defaultSkill = gameObject.transform.GetChild(0).AddComponent<BaseSkill>();
        }
        defaultSkill.Init();
    }

    /*외부 호출*/
    //=======================================//

    /// <summary>
    /// 스테이지 클리어 후 스킬 선택했을 때
    /// </summary>
    public void AddSkill(SkillData skillData)
    {
        // 이미 획득했는데 중복도 안되는 스킬이면 리턴
        if (!skillData.CanStack && AcquiredSkills.Contains(skillData))
            return;

        AcquiredSkills.Add(skillData);

        if (skillData.Type == SkillType.Status)
            ApplyPassiveSkill(skillData);
        else
            ApplyActiveSkill(skillData);

    }

    /// <summary>
    /// 플레이어 스크립트에서 호출, 공격
    /// </summary>
    /// <param name="launchPos"></param>
    /// <param name="direction"></param>
    public void ActivateSkills(Transform launchPos, Transform direction, int damage, float speed)
    {
        defaultSkill.Activate(launchPos, direction, damage, speed);
    }

    /*내부 로직*/
    //=======================================//
    private void ApplyPassiveSkill(SkillData skill)
    {
        switch (skill.statusType)
        {
                // increase dungeon attack
            case StatusType.Attack:
                playerStatus.IncreaseDungeonAtk(skill.skillValue);
                break;
            case StatusType.MaxHp:
                playerStatus.IncreaseDungeonMaxHp(skill.skillValue);
                break;
            case StatusType.Speed:
                playerStatus.IncreaseDungeonAttackSpeed(skill.skillValue);
                break;
            default:
                break;
        }
    }

    private void ApplyActiveSkill(SkillData skill)
    {
        switch (skill.ActiveSkillType)
        {
            case ActiveSkillType.ArrowCount:
                defaultSkill.ArrowCount += skill.ArrowCount;
                Debug.Log($"[PlayerSkills] {skill.SkillName}, 증가 화살 수:{skill.ArrowCount}, 현재 화살 수:{defaultSkill.ArrowCount}");
                break;
            case ActiveSkillType.ExtraPierce:
                defaultSkill.ExtraPierce += skill.ExtraPierce;
                Debug.Log($"[PlayerSkills] {skill.SkillName}, 증가 관통 수:{skill.ExtraPierce}, 현재 관통 수:{defaultSkill.ExtraPierce}");
                break;
            case ActiveSkillType.CoolDown:
                defaultSkill.CoolDown -= skill.CoolDown;
                Debug.Log($"[PlayerSkills] {skill.SkillName}, 감소 쿨 타임:{skill.CoolDown}, 현재 쿨 타임:{defaultSkill.CoolDown}");
                break;
        }
    }
}