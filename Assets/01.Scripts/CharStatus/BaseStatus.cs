using System;
using UnityEngine;

[System.Serializable]
public struct Status
{
    public int Hp;
    public int Atk;
}

public class BaseStatus : MonoBehaviour 
{
    /*필드 & 프로퍼티*/
    //=======================================//

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

        Debug.Log($"공격 {amount}, Hp {DungeonHp}");

        CheckDead();

        if (IsDead)
        {
            OnDead?.Invoke();
            Debug.Log("사망");
        }
        else
        {
            OnDamaged?.Invoke();
        }
    }

    private void CheckDead()
    {
        if (DungeonHp == 0) IsDead = true;
    }
}