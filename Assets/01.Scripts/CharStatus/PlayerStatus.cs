using System;
using UnityEngine;

public class PlayerStatus : BaseStatus
{
    /*필드 & 프로퍼티*/
    //=======================================//

    /*Dungeon*/
    public int DungeonExp { get; private set; }
    public int RequiredDungeonExp { get; private set; }
    //[SerializeField] private LevelData dungeonExpData;
    private ExpData dungeonExpData;

    /*Events*/
    public event Action OnInitDungeonPlayerFinished;

    public event Action OnDungeonLevelChanged;
    public event Action OnDungeonMaxHpChanged;
    public event Action OnDungeonHpChanged;
    public event Action OnDungeonAtkChanged;
    public event Action OnDungeonExpChanged;
    public event Action OnRequiredDungeonExpChanged;

    /*초기화 전용*/
    //=======================================//

    /// <summary>
    /// 던전 내부 스탯 초기화, 던전 진입마다 호출
    /// </summary>
    public void InitDungeon()
    {
        DungeonExp = 0;
        dungeonExpData = Resources.Load<ExpData>("DungeonLevelData");
        RequiredDungeonExp = dungeonExpData.ExpTable[DungeonLevel];

        DungeonLevel = 1;
        dungeonStatus = PlayerLobbyStatus.Instance.GetBaseData();
        DungeonHp = DungeonMaxHp = dungeonStatus.Hp;
        DungeonAtk = dungeonStatus.Atk;
        IsDead = false;

        OnInitDungeonPlayerFinished?.Invoke();
    }

    /*외부 호출용*/
    //=======================================//

    /*Dungeon*/
    public void IncreaseDungeonMaxHp(int amount) => InternalIncreaseDungeonMaxHp(amount);
    public void DecreseDungeonMaxHp(int amount) => InternalDecreseDungeonMaxHp(amount);
    public void IncreaseDungeonHp(int amount) => InternalIncreaseDungeonHp(amount);
    public void IncreaseDungeonAtk(int amount) => InternalIncreaseDungeonAtk(amount);
    public void DecreaseDungeonAtk(int amount) => InternalDecreaseDungeonAtk(amount);
    public void IncreaseDungeonExp(int amount) => InternalIncreaseDungeonExp(amount);

#if UNITY_EDITOR

    /*Dungeon*/
    public void EditorOnly_DecreaseDungeonExp(int amount) => InternalDecreaseDungeonExp(amount);
    public void EditorOnly_IncreaseDungeonLevel() => InternalIncreaseDungeonLevel();
    public void EditorOnly_DecreaseDungeonLevel() => InternalDecreaseDungeonLevel();
    public void DecreaseDungeonHp(int amount) => InternalDecreaseDungeonHp(amount);
#endif

    /*내부 로직*/
    //=======================================//
    
    private void InternalIncreaseDungeonMaxHp(int amount)
    {
        DungeonMaxHp += amount;
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

    private void InternalDecreaseDungeonHp(int amount)
    {
        DungeonHp -= amount;
        OnDungeonHpChanged?.Invoke();
    }

    private void InternalIncreaseDungeonAtk(int amount)
    {
        DungeonAtk += amount;
        OnDungeonAtkChanged?.Invoke();
    }

    private void InternalDecreaseDungeonAtk(int amount)
    {
        DungeonAtk -= amount;
        OnDungeonAtkChanged?.Invoke();
    }

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
            OnDungeonLevelChanged?.Invoke();
            // 만렙 체크
            if (DungeonLevel == dungeonExpData.ExpTable.Length)
            {
                DungeonExp = 0;
                return;
            }

            DungeonExp -= RequiredDungeonExp;
            // 다음 레벨이 요구하는 경험치로 기준치 상승
            RequiredDungeonExp = dungeonExpData.ExpTable[DungeonLevel];
            OnRequiredDungeonExpChanged?.Invoke();
        }
    }

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
}