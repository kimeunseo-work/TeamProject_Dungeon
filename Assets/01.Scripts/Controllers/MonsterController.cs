using UnityEditor.Rendering;
using UnityEngine;

public class MonsterController : BaseController
{
    /*�ʵ� & ������Ƽ*/
    //=======================================//

    private Transform target;
    private float followRange = 15f;
    private float attackRange = 0.8f;
    public float speed = 1f;

    /*�ʱ�ȭ*/
    //=======================================//

    public void Init(Transform target)
    {
        this.target = target;
    }

    /*�ܺ� ȣ��*/
    //=======================================//

    public override void HandleAction() // Monster �̵�
    {
        base.HandleAction();

        if (target == null)
        {
            if (!movementDirection.Equals(Vector2.zero)) movementDirection = Vector2.zero;

            CheckIsMoveChanged(movementDirection);
            return;
        }
        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();
        if (distance > attackRange)
        {
            if (distance <= followRange) // Ÿ���� ���� �ȿ� �ִ��� Ȯ��
            {
                lookDirection = direction;

                movementDirection = direction;
            }
        }
        else
        {
            movementDirection = Vector2.zero;
        }

        CheckIsMoveChanged(movementDirection);
    }
    protected override void Movement(Vector2 direction)
    {
         direction = direction * speed;
         _rigidbody.velocity = direction;
    }

    public override void Dead()
    {
    }

    /*���� ����*/
    //=======================================//

    protected float DistanceToTarget() // Target(Player)�� ���� ��ġ ���� �Ÿ�
    {
        return Vector3.Distance(transform.position, target.position);
        // �� ������ ���� �Ÿ�
    }

    protected Vector2 DirectionToTarget()
    {
        return (target.position - transform.position).normalized;
    }
}
