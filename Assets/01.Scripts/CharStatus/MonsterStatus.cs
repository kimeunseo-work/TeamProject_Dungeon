using System;
using UnityEngine;

public class MonsterStatus : BaseStatus 
{
    /*필드 & 프로퍼티*/
    //=======================================//

    public event Action OnInitDungeonMonsterFinished;

    /*초기화 전용*/
    //=======================================//

    public void InitDungeon(Status baseStatus, int dungeonLevel)
    {
        DungeonLevel = dungeonLevel;
        dungeonStatus = baseStatus;
        DungeonMaxHp = dungeonStatus.Hp;

        DungeonHp = dungeonStatus.Hp * DungeonLevel;
        DungeonAtk = dungeonStatus.Atk * DungeonLevel;

        IsDead = false;

        OnInitDungeonMonsterFinished?.Invoke();
    }

    public override void TakeDamage(int amount)
    {
        var prevHp = DungeonHp;
        base.TakeDamage(amount);
        Debug.Log($" [{nameof(MonsterStatus)}] monster takeDamage = {amount}. prevHp = {prevHp}, currentHp = {DungeonHp}");
    }
}