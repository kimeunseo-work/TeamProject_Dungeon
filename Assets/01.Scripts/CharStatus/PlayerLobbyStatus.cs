using System;
using UnityEngine;

public class PlayerLobbyStatus : MonoBehaviour 
{
    /*필드 & 프로퍼티*/
    //=======================================//

    public static PlayerLobbyStatus Instance;

    private PlayerData playerData;
    private ExpData baseExpData;
    private Status baseStatus;
    
    /*Base*/
    public string Name { get; private set; }
    public int BaseLevel { get; private set; }
    public int BaseExp { get; private set; }
    public int RequiredBaseExp { get; private set; }
    public int Point { get; private set; }

    public int BaseHP
    {
        get => baseStatus.Hp;
        private set => baseStatus.Hp = value;
    }
    public int BaseATK
    {
        get => baseStatus.Atk;
        private set => baseStatus.Atk = value;
    }

    /*Events*/
    public event Action OnInitAccountFinished;

    public event Action OnBaseLevelChanged;
    public event Action OnBaseHpChanged;
    public event Action OnBaseAtkChanged;
    public event Action OnBaseExpChanged;
    public event Action OnRequiredBaseExpChanged;
    public event Action OnPointChanged;

    /*생명 주기*/
    //=======================================//

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        playerData = Resources.Load<PlayerData>("PlayerData");
        baseExpData = Resources.Load<ExpData>("BaseLevelData");

        InitAccount();
    }

    /*초기화 전용*/
    //=======================================//

    /// <summary>
    /// 계정 생성시 초기화, 최초 1번 호출
    /// </summary>
    public void InitAccount()
    {
        Name = playerData.name;
        this.baseStatus = new Status {Hp = playerData.Hp, Atk = playerData.Atk };
        BaseHP = this.baseStatus.Hp;
        BaseATK = this.baseStatus.Atk;

        BaseLevel = 1;
        BaseExp = 0;
        RequiredBaseExp = baseExpData.ExpTable[BaseLevel];
        Point = 0;

        OnInitAccountFinished?.Invoke();
    }

    public Status GetBaseData() { return baseStatus; }

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

#if UNITY_EDITOR
    /*Base*/
    public void EditorOnly_DecreaseBaseHp() => InternalDecreaseBaseHp();
    public void EditorOnly_DecreaseBaseAtk() => InternalDecreaseBaseAtk();
    public void EditorOnly_DecreaseBaseExp(int amount) => InternalDecreaseBaseExp(amount);
    public void EditorOnly_IncreaseLevel() => InternalIncreseBaseLevel();
    public void EditorOnly_DecreaseLevel() => InternalDecreseBaseLevel();
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
}