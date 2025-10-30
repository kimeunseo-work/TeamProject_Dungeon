using UnityEngine;

public class MonsterController : BaseController
{
    /*필드 & 프로퍼티*/
    //=======================================//

    private Transform target;
    private float followRange = 15f;

    /*초기화*/
    //=======================================//

    public void Init(Transform target)
    {
        this.target = target;
    }

    /*외부 호출*/
    //=======================================//

    public override void HandleAction() // Monster 이동
    {
        base.HandleAction();

        if (target == null)
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

    public override void Dead()
    {
    }

    /*내부 로직*/
    //=======================================//

    protected float DistanceToTarget() // Target(Player)과 현재 위치 사이 거리
    {
        return Vector3.Distance(transform.position, target.position);
        // 두 포지션 사이 거리
    }

    protected Vector2 DirectionToTarget() // Target(Player)에게 다가가는 속도
    {
        return (target.position - transform.position).normalized;
    }
}
