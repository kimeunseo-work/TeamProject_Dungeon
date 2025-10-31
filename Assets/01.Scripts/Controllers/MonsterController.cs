using System.Runtime.CompilerServices;
using UnityEngine;

public class MonsterController : BaseController
{
    /*필드 & 프로퍼티Ƽ*/
    //=======================================//
    public enum MonsterType
    {
        Melee = 0,
        Ranged = 1,
    }
    public MonsterType type;
    private Transform target;
    public GameObject shooter;

    private float followRange = 15f;
    public float attackRange = 0.8f;
    public float speed = 1f;
    

    /*초기화*/
    //=======================================//

    public void Init(Transform target)
    {
        this.target = target;
    }

    /*외부 호출용*/
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
            MonsterAttack();
            movementDirection = Vector2.zero;
        }

        CheckIsMoveChanged(movementDirection);
    }
    private void MonsterAttack()
    {
        float shootSpeed = 0.2f;
        if(type == MonsterType.Melee)
        { 
            // 근거리 몬스터 공격
        }
        else if(type == MonsterType.Ranged)
        {
            if (shooter == null) return;

            Vector2 direction = DirectionToTarget();

            GameObject Shot = Instantiate(shooter, transform.position, Quaternion.identity);

            Rigidbody2D _rigidbody = shooter.GetComponent<Rigidbody2D>();
            _rigidbody.velocity = direction * shootSpeed;
        }    
    }
    protected override void Movement(Vector2 direction)
    {
         direction = direction * speed;
         _rigidbody.velocity = direction;
    }

    public override void Dead()
    {
    }

    /*내부 로직*/
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
