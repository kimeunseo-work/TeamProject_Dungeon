using System.Collections.Generic;
using UnityEngine;
public class PlayerSkills : MonoBehaviour
{
    [SerializeField] public List<SkillData> acquiredSkills = new List<SkillData>();
    [SerializeField] private PlayerStatus playerStatus;
    public static PlayerSkills Instance;
    [SerializeField] private SkillData defaultSkilldata;
    [SerializeField] private BaseSkill defaultSkill;

    private void Start()
    {
        SkillManager.Instance.Init(this);
    }

    private void Reset()
    {
        dungeonStatus = GetComponent<PlayerStatus>();
        defaultSkill = GetComponent<BaseSkill>();
    private void Update()
    {
        {
        if (Input.GetKeyDown(KeyCode.G))
            playerStatus.IncreaseDungeonExp(1);
        }
    }

    private void Start()
    {
        if(defaultSkill == null)
        {
            Debug.Log("[PlayerSkills] defaultSkill is NULL");
        }
        defaultSkill.Init();
    }

    /// <summary>
    /// 현재 등록된 스킬
    /// UI에서 랜덤 스킬 뽑을 때 중복을 피하기 위해서 사용
    /// </summary>
    public List<SkillData> AcquiredSkills { get; private set; } = new(5);


    /*외부 호출*/
    //=======================================//

    /// <summary>
    /// 스테이지 클리어 후 스킬 선택했을 때
    /// </summary>
    public void AddSkill(SkillData skill)
    {
        // 이미 획득했는데 중복도 안되는 스킬이면 리턴
        if (!skill.CanStack && AcquiredSkills.Contains(skill))
            return;

        AcquiredSkills.Add(skill);

        if (skill.Type == SkillType.Passive)
            ApplyPassiveSkill(skill);
        else
            ApplyActiveSkill(skill);
    }

    /// <summary>
    /// 플레이어 스크립트에서 호출, 공격
    /// </summary>
    /// <param name="launchPos"></param>
    /// <param name="direction"></param>
    public void ActivateSkills(Transform launchPos, Transform direction)
    {
        defaultSkill.Activate(launchPos, direction);
    }

    /*내부 로직*/
    //=======================================//

    private void ApplyPassiveSkill(SkillData skill)
    {
        switch (skill.SkillName)
        {
            case "Increase Attack":
                // increase dungeon attack
                //PlayerLobbyStatus.Instance.IncreaseBaseAtk();
                dungeonStatus.IncreaseDungeonAtk(1);
                break;
            case "Increase Hp":
                // increase dungeon hp
                //PlayerLobbyStatus.Instance.IncreaseBaseHp();
                dungeonStatus.IncreaseDungeonMaxHp(1);
                break;
            case "Increase Attack Speed":
                // increase dungeon attack Speed
                break;
        }
    }

    private void ApplyActiveSkill(SkillData skill)
    {
        switch (skill.ActiveSkillType)
        {
            case ActiveSkillType.ArrowCount:
                defaultSkill.ArrowCount += skill.ArrowCount;
                Debug.Log($"[PlayerSkills] {skill.SkillName}, 증가 화살 수:{skill.ArrowCount}, 현재 화살 수:{skill.ArrowCount}");
                break;
            case ActiveSkillType.ExtraPierce:
                defaultSkill.ExtraPierce += skill.ExtraPierce;
                Debug.Log($"[PlayerSkills] {skill.SkillName}, 증가 관통 수:{skill.ExtraPierce}, 현재 관통 수:{skill.ExtraPierce}");
                break;
            case ActiveSkillType.CoolDown:
                defaultSkill.CoolDown -= skill.CoolDown;
                Debug.Log($"[PlayerSkills] {skill.SkillName}, 감소 쿨 타임:{skill.CoolDown}, 현재 쿨 타임:{skill.CoolDown}");
                break;
        }
    }
}