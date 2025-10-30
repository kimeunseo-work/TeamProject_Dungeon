using System;

public class MonsterStatus : BaseStatus
{
    /*필드 & 프로퍼티*/
    //=======================================//

    public event Action OnInitDungeonMonsterFinished;

    /*초기화 전용*/
    //=======================================//

    public void InitDungeon(string name, Status baseStatus, int dungeonLevel)
    {
        Name = name;
        this.baseStatus = baseStatus;
        BaseHP = this.baseStatus.Hp;
        BaseATK = this.baseStatus.Atk;

        DungeonLevel = dungeonLevel;
        dungeonStatus = this.baseStatus;
        DungeonMaxHp = dungeonStatus.Hp;

        SetStatus();

        IsDead = false;

        OnInitDungeonMonsterFinished?.Invoke();
    }

    /*내부 로직*/
    //=======================================//

    /// <summary>
    /// 몬스터 레벨에 따른 스탯 설정 메서드
    /// </summary>
    private void SetStatus()
    {
        DungeonHp = BaseHP * DungeonLevel;
        DungeonAtk = BaseATK * DungeonLevel;
    }
}