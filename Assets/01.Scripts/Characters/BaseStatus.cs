using System;
using UnityEngine;

public class BaseStatus : MonoBehaviour
{
    /*내부 구조체 & 클래스*/
    //=======================================//
    public struct Status
    {
        public int Hp;
        public int Atk;
    }

    /*필드 & 프로퍼티*/
    //=======================================//

    public string Name { get; protected set; }

    /*Base*/
    protected Status baseStatus;
    public int BaseHP
    {
        get => baseStatus.Hp;
        set => baseStatus.Hp = value;
    }
    public int BaseATK
    {
        get => baseStatus.Atk;
        set => baseStatus.Atk = value;
    }

    /*Dungeon*/
    protected Status dungeonStatus;
    public int DungeonLevel { get; protected set; }
    public int DungeonMaxHp { get; protected set; }
    public int DungeonHp
    {
        get => dungeonStatus.Hp;
        protected set
        {
            if (value <= 0)
            {
                dungeonStatus.Hp = 0;
            }
            else if (value >= DungeonMaxHp)
            {
                dungeonStatus.Hp = DungeonMaxHp;
            }
            else
            {
                dungeonStatus.Hp = value;
            }
        }
    }
    public int DungeonAtk
    {
        get => dungeonStatus.Atk;
        protected set => dungeonStatus.Atk = value;
    }
    public bool IsDead { get; protected set; }

    /*Events*/
    public event Action OnInitDungeonFinished;

    public event Action OnDead;
    public event Action OnDamaged;

    /*외부 호출용*/
    //=======================================//

    /// <summary>
    /// 공격 받았을 때 호출
    /// </summary>
    public void TakeDamage(int amount) => DecreaseDungeonHp(amount);

    /*내부 로직*/
    //=======================================//
    private void DecreaseDungeonHp(int amount)
    {
        DungeonHp -= amount;
        CheckDead();

        if (IsDead)
        {
            OnDead?.Invoke();
            Debug.Log($"{Name} IsDead = {IsDead}.");
            Debug.Log($"{Name} currentDungeonHp = {DungeonHp}");
        }
        else
        {
            OnDamaged?.Invoke();
            Debug.Log($"{Name} take damage = {amount}");
            Debug.Log($"{Name} currentDungeonHp = {DungeonHp}");
        }
    }

    private void CheckDead()
    {
        if (DungeonHp == 0) IsDead = true;
    }
}