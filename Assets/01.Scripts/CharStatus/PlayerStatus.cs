using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : BaseStatus
{
    /*필드 & 프로퍼티*/
    //=======================================//

    /*Dungeon*/
    public int DungeonExp { get; private set; }
    public int RequiredDungeonExp { get; private set; }
    private ExpData dungeonExpData;

    /*Skills*/
    //List<PassiveSkill> passiveSkills;
    //List<Skill> arrowSkills;
    //BaseSkill defaultSkill = new();

    /*Events*/
    public event Action OnInitDungeonPlayerFinished;

    public event Action OnDungeonLevelChanged;
    public event Action OnDungeonMaxHpChanged;
    public event Action OnDungeonHpChanged;
    public event Action OnDungeonAtkChanged;
    public event Action OnDungeonExpChanged;
    public event Action OnRequiredDungeonExpChanged;

    //public event Action OnSkillsListChanged;

    /*초기화 전용*/
    //=======================================//

    /// <summary>
    /// 던전 내부 스탯 초기화, 던전 진입마다 호출
    /// </summary>
    public void InitDungeon()
    {
        DungeonLevel = 1;
        IsDead = false;

        /*Exp Init*/
        DungeonExp = 0;
        dungeonExpData = Resources.Load<ExpData>("DungeonLevelData");
        RequiredDungeonExp = dungeonExpData.ExpTable[DungeonLevel];

        /*Status Init*/
        dungeonStatus = PlayerLobbyStatus.Instance.GetBaseData();
        DungeonHp = DungeonMaxHp = dungeonStatus.Hp;
        DungeonAtk = dungeonStatus.Atk;

        /*Skill Init*/
        //defaultSkill.Init(Resources.Load<SkillData>("defaultSkill"));

        OnInitDungeonPlayerFinished?.Invoke();
    }

    /*외부 호출용*/
    //=======================================//

    public override void TakeDamage(int amount)
    {
        var prevHp = DungeonHp;
        base.TakeDamage(amount);
        Debug.Log($" [{nameof(MonsterStatus)}] player takeDamage = {amount}. prevHp = {prevHp}, currentHp = {DungeonHp}");
    }

    /*Status*/
    public void IncreaseDungeonMaxHp(int amount) => InternalIncreaseDungeonMaxHp(amount);
    public void DecreseDungeonMaxHp(int amount) => InternalDecreseDungeonMaxHp(amount);
    public void IncreaseDungeonHp(int amount) => InternalIncreaseDungeonHp(amount);
    public void IncreaseDungeonAtk(int amount) => InternalIncreaseDungeonAtk(amount);
    public void DecreaseDungeonAtk(int amount) => InternalDecreaseDungeonAtk(amount);

    /*Level & Exp*/
    public void IncreaseDungeonExp(int amount) => InternalIncreaseDungeonExp(amount);

    /*Skills*/

    //public void AddPassiveSkill(PassiveSkill skill) => InternalAddPassiveSkills(skill);
    //public void AddArrowSkill(BaseSkill skill) => InternalAddSkill(skill);
    //public void ActiveDefaultSkill(SkillData skill, Transform launchPos, Transform direction) => defaultSkill.Activate(skill, launchPos, direction);
    //public void UpgradeSkill(SkillData skillData) => InternalUpgradeSkill(skillData);


#if UNITY_EDITOR

    /*Status*/
    /// <summary>
    /// 대신 TakeDamage를 사용하세요.
    /// </summary>
    public void EditorOnly_DecreaseDungeonHp(int amount) => InternalDecreaseDungeonHp(amount);

    /*Level & Exp*/
    public void EditorOnly_DecreaseDungeonExp(int amount) => InternalDecreaseDungeonExp(amount);
    public void EditorOnly_IncreaseDungeonLevel() => InternalIncreaseDungeonLevel();
    public void EditorOnly_DecreaseDungeonLevel() => InternalDecreaseDungeonLevel();

    /*Skills*/
    //public void EditorOnly_RemovePassiveSkill(PassiveSkill skill) => InternalRemovePassiveSkill(skill);
    //public void EditorOnly_RemoveArrowSkill(BaseSkill skill) => InternalRemoveSkill(skill);

#endif

    /*내부 로직*/
    //=======================================//

    /*Status*/
    private void InternalIncreaseDungeonMaxHp(int amount)
    {
        var ex = DungeonMaxHp;
        DungeonMaxHp += amount;
        Debug.Log($"[PlayerStatus] Before MaxHp = {ex}, After MaxHp = {DungeonMaxHp}");
        OnDungeonMaxHpChanged?.Invoke();
    }

    private void InternalDecreseDungeonMaxHp(int amount)
    {
        DungeonMaxHp -= amount;
        OnDungeonMaxHpChanged?.Invoke();

        if (DungeonHp > DungeonMaxHp)
        {
            DungeonHp = DungeonMaxHp;
            OnDungeonHpChanged?.Invoke();
        }
    }

    private void InternalIncreaseDungeonHp(int amount)
    {
        DungeonHp += amount;
        OnDungeonHpChanged?.Invoke();
    }

    private void InternalIncreaseDungeonAtk(int amount)
    {
        var ex = DungeonAtk;
        DungeonAtk += amount;
        Debug.Log($"[PlayerStatus] Before MaxHp = {ex}, After MaxHp = {DungeonAtk}");
        OnDungeonAtkChanged?.Invoke();
    }

    private void InternalDecreaseDungeonAtk(int amount)
    {
        DungeonAtk -= amount;
        OnDungeonAtkChanged?.Invoke();
    }

    /*Level & Exp*/
    private void InternalIncreaseDungeonExp(int amount)
    {
        // 만렙 체크
        if (DungeonLevel == dungeonExpData.ExpTable.Length) return;

        DungeonExp += amount;
        OnDungeonExpChanged?.Invoke();

        CheckDungeonLevelUp();
    }

    private void CheckDungeonLevelUp()
    {
        while (DungeonExp >= RequiredDungeonExp)
        {
            DungeonLevel++;
            // 만렙 체크
            if (DungeonLevel == dungeonExpData.ExpTable.Length)
            {
                DungeonExp = 0;
                return;
            }

            DungeonExp -= RequiredDungeonExp;
            // 다음 레벨이 요구하는 경험치로 기준치 상승
            RequiredDungeonExp = dungeonExpData.ExpTable[DungeonLevel];

            OnDungeonLevelChanged?.Invoke();
            OnRequiredDungeonExpChanged?.Invoke();
        }
    }

    /*Skills*/

    //private void InternalAddSkill(BaseSkill skill)
    //{
    //    skills.Add(skill);
    //    OnSkillsListChanged?.Invoke();

    //    Debug.Log($"[PlayerStatus] player gain arrowSkill: {skill.SkillName}? {skill.name}");
    //}
    //private void InternalAddPassiveSkills(PassiveSkill skill)
    //{
    //    passiveSkills.Add(skill);
    //    OnSkillsListChanged?.Invoke();

    //    Debug.Log($"[PlayerStatus] player gain passiveSkill: {skill.skillName}? {skill.name}");
    //}
    //private void InternalUpgradeSkill(SkillData data)
    //{
    //    defaultSkill.ArrowCount += data.ArrowCount;
    //    defaultSkill.ExtraPierce += data.ExtraPierce;
    //    defaultSkill.CoolDown -= data.CoolDown;

    //    OnSkillsListChanged?.Invoke();
    //}

#if UNITY_EDITOR
    /*Status*/
    private void InternalDecreaseDungeonHp(int amount)
    {
        DungeonHp -= amount;
        OnDungeonHpChanged?.Invoke();
    }

    /*Level & Exp*/
    private void InternalDecreaseDungeonExp(int amount)
    {
        DungeonExp -= amount;
        OnDungeonExpChanged?.Invoke();
    }
    private void InternalIncreaseDungeonLevel()
    {
        DungeonLevel++;
        OnDungeonLevelChanged?.Invoke();
    }
    private void InternalDecreaseDungeonLevel()
    {
        DungeonLevel--;
        OnDungeonLevelChanged?.Invoke();
    }

    /*Skills*/
    //private void InternalRemoveSkill(BaseSkill skill)
    //{
    //    skills.Remove(skill);
    //    OnSkillsListChanged?.Invoke();

    //    Debug.Log($"[PlayerStatus] player lose arrowSkill: {skill.SkillName}? {skill.name}");
    //}

    //private void InternalRemovePassiveSkill(PassiveSkill skill)
    //{
    //    passiveSkills.Remove(skill);
    //    OnSkillsListChanged?.Invoke();

    //    Debug.Log($"[PlayerStatus] player lose arrowSkill: {skill.skillName}? {skill.name}");
    //}
#endif
}