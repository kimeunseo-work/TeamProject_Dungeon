using UnityEngine;

public class PlayerStatus : BaseStatus
{
    /*필드 & 프로퍼티*/
    //=======================================//

    /*Base*/
    public int BaseLevel { get; private set; }
    public int BaseExp { get; private set; }
    public int RequiredBaseExp { get; private set; }
    //[SerializeField] private LevelData baseExpData;
    public ExpData baseExpData;
    public int Point { get; private set; }

    /*Dungeon*/
    public int DungeonExp { get; private set; }
    public int RequiredDungeonExp { get; private set; }
    //[SerializeField] private LevelData dungeonExpData;
    public ExpData dungeonExpData;

    /*초기화 전용*/
    //=======================================//

    /// <summary>
    /// 계정 생성시 초기화, 최초 1번 호출
    /// </summary>
    public void InitAccount(string name, Status baseStatus)
    {
        Name = name;
        this.baseStatus = baseStatus;
        BaseHP = this.baseStatus.Hp;
        BaseATK = this.baseStatus.Atk;

        BaseLevel = 1;
        BaseExp = 0;
        RequiredBaseExp = baseExpData.ExpTable[BaseLevel];
        Point = 0;
    }

    /// <summary>
    /// 던전 내부 스탯 초기화, 던전 진입마다 호출
    /// </summary>
    public void InitDungeon()
    {
        DungeonExp = 0;
        RequiredDungeonExp = dungeonExpData.ExpTable[DungeonLevel];

        DungeonLevel = 1;
        dungeonStatus = baseStatus;
        DungeonHp = DungeonMaxHp = dungeonStatus.Hp;
        DungeonAtk = dungeonStatus.Atk;
        IsDead = false;
    }

#if UNITY_EDITOR
    public void EditorOnly_AccountInit(int baseLevel, int baseExp, int point, Status baseStatus)
    {
        BaseLevel = baseLevel;
        BaseExp = baseExp;
        RequiredBaseExp = baseExpData.ExpTable[BaseLevel];
        Point = point;

        this.baseStatus = baseStatus;
        BaseHP = this.baseStatus.Hp;
        BaseATK = this.baseStatus.Atk;
    }
#endif

    /*외부 호출용*/
    //=======================================//

    /*Base*/
    public void IncreaseBaseHp() => BaseHP++;
    public void IncreaseBaseAtk() => BaseATK++;
    public void IncreaseBaseExp(int amount) => AddBaseExp(amount);
    public void IncreasePoint() => Point++;
    public void DecreasePoint() => Point--;

    /*Dungeon*/
    public void IncreaseDungeonMaxHp(int amount) => DungeonMaxHp += amount;
    public void DecreseDungeonMaxHp(int amount) => MinusDungeonMaxHp(amount);
    public void IncreaseDungeonHp(int amount) => DungeonHp += amount;
    //public void DecreaseDungeonHp(int amount) => DungeonHp -= amount;
    public void IncreaseDungeonAtk(int amount) => DungeonAtk += amount;
    public void DecreaseDungeonAtk(int amount) => DungeonAtk -= amount;
    public void IncreaseDungeonExp(int amount) => AddDungeonExp(amount);

#if UNITY_EDITOR
    /*Base*/
    public void EditorOnly_DecreaseBaseHp() => BaseHP--;
    public void EditorOnly_DecreaseBaseAtk() => BaseATK--;
    public void EditorOnly_DecreaseBaseExp(int amount) => BaseExp -= amount;
    public void EditorOnly_IncreaseLevel() => BaseLevel++;
    public void EditorOnly_DecreaseLevel() => BaseLevel--;

    /*Dungeon*/
    public void EditorOnly_DecreaseDungeonExp(int amount) => DungeonExp -= amount;
    public void EditorOnly_IncreaseDungeonLevel() => DungeonLevel++;
    public void EditorOnly_DecreaseDungeonLevel() => DungeonLevel--;
#endif

    /*내부 로직*/
    //=======================================//

    private void AddBaseExp(int amount)
    {
        // 만렙 체크
        if (BaseLevel == baseExpData.ExpTable.Length) return;
        
        BaseExp += amount;
        CheckBaseLevelUp();
    }

    private void CheckBaseLevelUp()
    {
        while (BaseExp >= RequiredBaseExp)
        {
            BaseLevel++;

            // 만렙 체크
            if (BaseLevel == baseExpData.ExpTable.Length)
            {
                BaseExp = 0;
                return;
            }

            BaseExp -= RequiredBaseExp;
            // 다음 레벨이 요구하는 경험치로 기준치 상승
            RequiredBaseExp = baseExpData.ExpTable[BaseLevel];
        }
    }

    private void AddDungeonExp(int amount)
    {
        // 만렙 체크
        if (DungeonLevel == dungeonExpData.ExpTable.Length) return;

        DungeonExp += amount;
        CheckDungeonLevelUp();
    }

    private void CheckDungeonLevelUp()
    {
        while (DungeonExp >= RequiredDungeonExp)
        {
            DungeonLevel++;

            // 만렙 체크
            if (BaseLevel == baseExpData.ExpTable.Length)
            {
                DungeonExp = 0;
                return;
            }

            DungeonExp -= RequiredDungeonExp;
            // 다음 레벨이 요구하는 경험치로 기준치 상승
            RequiredDungeonExp = dungeonExpData.ExpTable[DungeonLevel];
        }
    }

    private void MinusDungeonMaxHp(int amount)
    {
        DungeonMaxHp -= amount;
        DungeonHp = Mathf.Min(DungeonHp, DungeonMaxHp);
    }
}