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

    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.position);
        // 두 포지션 사이 거리
    }

    protected Vector2 DirectionToTarget()
    {
        return (target.position - transform.position).normalized;
    }

    protected override void HandleAction()
    {
        base.HandleAction();

        if(target == null)
        {
            if (!movementDirection.Equals(Vector2.zero)) movementDirection = Vector2.zero;
            return;
        }
        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        if (distance < followRange)
        {
            lookDirection = direction;

            movementDirection = direction;
        }
    }
}
