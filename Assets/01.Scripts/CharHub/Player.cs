using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    /*필드 & 프로퍼티*/
    //=======================================// 

    // 플레이어
    [SerializeField] private PlayerStatus status;
    [SerializeField] private PlayerController controller;
    [SerializeField] private AutoArrowSkill autoArrow;

    /*Monsters*/
    private List<Transform> currentEnemyTrans = new(10);
    private List<MonsterStatus> currentEnemyStatus = new(10);

    /*Events*/
    public event Action<bool> OnCanAttackChanged;

    /*생명 주기*/
    //=======================================//

    private void Awake()
    {
        // 스탯 초기화
        status.InitDungeon();
    }

    private void OnEnable()
    {
        status.OnDead += Status_OnDead;
        controller.OnMoveChanged += Controller_OnMoveChanged;
    }

    private void OnDisable()
    {
        status.OnDead -= Status_OnDead;
        controller.OnMoveChanged -= Controller_OnMoveChanged;
    }

    protected override void Update()
    {
        // Attack()
        base.Update();

        // 플레이어 입력 받기
        controller.HandleAction();
        // 가까운 적 찾기
        FindNearEnemy();
    }

    /*외부 호출*/
    //=======================================//

    /// <summary>
    /// 몬스터, 함정 오브젝트 전용
    /// </summary>
    public override void TakeDamage(int amount)
    {
        // 데이터
        status.TakeDamage(amount);
        // 피격 액션
        //controller.TakeDamage(amount);
    }

    /// <summary>
    /// 던전 경험치 오브젝트 전용
    /// </summary>
    public void GetDungeonExp(int amount)
    {
        status.IncreaseDungeonExp(amount);
        Debug.Log($"획득 경험치 {amount}, 현재 경험치 {status.DungeonExp}");
    }

    /// <summary>
    /// 다음 맵으로 넘어갈 때 마다 호출
    /// StageManager 전용
    /// </summary>
    public void FindEnemy()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        currentEnemyTrans.Clear();
        currentEnemyStatus.Clear();

        foreach (var e in enemies)
        {
            currentEnemyTrans.Add(e.transform);
            currentEnemyStatus.Add(e.GetComponent<MonsterStatus>());
        }
    }

    /*내부 로직*/
    //=======================================//

    protected override void Attack()
    {

    }

    /// <summary>
    /// 가장 가까운 적 찾는 로직
    /// </summary>
    private void FindNearEnemy()
    {
        Transform nearEnemy = null;
        var minDist = float.MaxValue;
        var thisPos = (Vector2)transform.position;

        for (int i = 0; i < currentEnemyTrans.Count; i++)
        {
            var status = currentEnemyStatus[i];
            if (status == null || status.IsDead) continue;

            float dist = Vector2.Distance(thisPos, currentEnemyTrans[i].position);
            
            if (dist < minDist)
            {
                minDist = dist;
                nearEnemy = currentEnemyTrans[i];
            }
        }

        TargetTransform = nearEnemy;
    }

    /*이벤트 구독*/
    //=======================================//

    protected override void Status_OnDead()
    {
        // 사망 액션
        //controller.Dead(amount);
        
        // 삭제(나중에 시간되면 오브젝트 풀링 사용?)
        Destroy(gameObject);
    }
    private void Controller_OnMoveChanged(bool isMove)
    {
        CanAttack = !isMove;
        OnCanAttackChanged?.Invoke(CanAttack);
    }
}