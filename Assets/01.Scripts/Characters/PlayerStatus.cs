using System;

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

    /*Events*/
    public event Action OnInitAccountFinished;
    public event Action OnInitDungeonPlayerFinished;

    public event Action OnBaseLevelChanged;
    public event Action OnBaseHpChanged;
    public event Action OnBaseAtkChanged;
    public event Action OnBaseExpChanged;
    public event Action OnRequiredBaseExpChanged;
    public event Action OnPointChanged;
    public event Action OnDungeonLevelChanged;
    public event Action OnDungeonMaxHpChanged;
    public event Action OnDungeonHpChanged;
    public event Action OnDungeonAtkChanged;
    public event Action OnDungeonExpChanged;
    public event Action OnRequiredDungeonExpChanged;

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

        OnInitAccountFinished?.Invoke();
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

        OnInitDungeonPlayerFinished?.Invoke();
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
    public void IncreaseBaseHp() => InternalIncreaseBaseHp();
    public void IncreaseBaseAtk() => InternalIncreaseBaseAtk();
    public void IncreaseBaseExp(int amount) => InternalIncreaseBaseExp(amount);
    public void IncreasePoint() => InternalIncreasePoint();
    public void DecreasePoint() => InternalDecreasePoint();

    /*Dungeon*/
    public void IncreaseDungeonMaxHp(int amount) => InternalIncreaseDungeonMaxHp(amount);
    public void DecreseDungeonMaxHp(int amount) => InternalDecreseDungeonMaxHp(amount);
    public void IncreaseDungeonHp(int amount) => InternalIncreaseDungeonHp(amount);
    public void IncreaseDungeonAtk(int amount) => InternalIncreaseDungeonAtk(amount);
    public void DecreaseDungeonAtk(int amount) => InternalDecreaseDungeonAtk(amount);
    public void IncreaseDungeonExp(int amount) => InternalIncreaseDungeonExp(amount);

#if UNITY_EDITOR
    /*Base*/
    public void EditorOnly_DecreaseBaseHp() => InternalDecreaseBaseHp();
    public void EditorOnly_DecreaseBaseAtk() => InternalDecreaseBaseAtk();
    public void EditorOnly_DecreaseBaseExp(int amount) => InternalDecreaseBaseExp(amount);
    public void EditorOnly_IncreaseLevel() => InternalIncreseBaseLevel();
    public void EditorOnly_DecreaseLevel() => InternalDecreseBaseLevel();

    /*Dungeon*/
    public void EditorOnly_DecreaseDungeonExp(int amount) => InternalDecreaseDungeonExp(amount);
    public void EditorOnly_IncreaseDungeonLevel() => InternalIncreaseDungeonLevel();
    public void EditorOnly_DecreaseDungeonLevel() => InternalDecreaseDungeonLevel();
    public void DecreaseDungeonHp(int amount) => InternalDecreaseDungeonHp(amount);
#endif

    /*내부 로직*/
    //=======================================//

    private void InternalIncreaseBaseHp()
    {
        BaseHP++;
        OnBaseHpChanged?.Invoke();
    }

    private void InternalDecreaseBaseHp()
    {
        BaseHP--;
        OnBaseHpChanged?.Invoke();
    }

    private void InternalIncreaseBaseAtk()
    {
        BaseATK++;
        OnBaseAtkChanged?.Invoke();
    }

    private void InternalDecreaseBaseAtk()
    {
        BaseATK--;
        OnBaseAtkChanged?.Invoke();
    }

    private void InternalIncreaseBaseExp(int amount)
    {
        // 만렙 체크
        if (BaseLevel == baseExpData.ExpTable.Length) return;
        
        BaseExp += amount;
        OnBaseExpChanged?.Invoke();

        CheckBaseLevelUp();
    }

    private void CheckBaseLevelUp()
    {
        while (BaseExp >= RequiredBaseExp)
        {
            BaseLevel++;
            OnBaseLevelChanged.Invoke();

            // 만렙 체크
            if (BaseLevel == baseExpData.ExpTable.Length)
            {
                BaseExp = 0;
                return;
            }

            BaseExp -= RequiredBaseExp;
            // 다음 레벨이 요구하는 경험치로 기준치 상승
            RequiredBaseExp = baseExpData.ExpTable[BaseLevel];
            OnRequiredBaseExpChanged?.Invoke();
        }
    }

    private void InternalDecreaseBaseExp(int amount)
    {
        BaseExp -= amount;
        OnBaseExpChanged?.Invoke();
    }
    
    private void InternalIncreseBaseLevel()
    {
        BaseLevel++;
        OnBaseLevelChanged?.Invoke();
    }

    private void InternalDecreseBaseLevel()
    {
        BaseLevel--;
        OnBaseLevelChanged?.Invoke();
    }

    private void InternalIncreasePoint()
    {
        Point++;
        OnPointChanged?.Invoke();
    }

    private void InternalDecreasePoint()
    {
        Point--;
        OnPointChanged?.Invoke();
    }

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
            if (BaseLevel == baseExpData.ExpTable.Length)
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