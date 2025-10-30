using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : BaseController
{
    [SerializeField] private Transform target;

    [SerializeField] private float followRange = 15f;

    //public void Init(Transform target)
    //{
    //    this.target = target;
    //}

    protected float DistanceToTarget() // Target(Player)과 현재 위치 사이 거리
    {
        return Vector3.Distance(transform.position, target.position);
        // 두 포지션 사이 거리
    }

    protected Vector2 DirectionToTarget() // Target(Player)에게 다가가는 속도
    {
        return (target.position - transform.position).normalized;
    }

    protected override void HandleAction() // Monster 이동
    {
        base.HandleAction();

        if(target == null)
        {
            if (!movementDirection.Equals(Vector2.zero)) movementDirection = Vector2.zero;
            return;
        }
        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        if (distance <= followRange) // 타겟이 범위 안에 있는지 확인
        {
            lookDirection = direction;

            movementDirection = direction;
        }
    }

    protected override void Dead()
    {
        // Destroy(GameObject.Monster);
    }
}
