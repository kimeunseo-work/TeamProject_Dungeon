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
    public MonsterType Type;
    private Transform target;
    public GameObject ShooterPrefab;

    private readonly float followRange = 15f;
    public float AttackRange = 0.8f;
    public float speed = 1f; // 대문자 바꾸면 오버라이드 때문에 그냥 내버려 둠
    private readonly float cooldown = 3.0f;
    private float timer;


    /*초기화*/
    //=======================================//

    public void Init(Transform target)
    {
        this.target = target;
    }

    /*외부 호출용*/
    //=======================================//

    public override void HandleAction() // Monster
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
        if (distance > AttackRange)
        {
            if (distance <= followRange)
            {
                lookDirection = direction;

                movementDirection = direction;
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer <= cooldown) return;
            else
            {
                MonsterAttack();
                timer = 0f;
                movementDirection = Vector2.zero;
            }
        }

        CheckIsMoveChanged(movementDirection);
    }
    private void MonsterAttack()
    {
        float shootSpeed = 0.2f;
        if (Type == MonsterType.Melee)
        {
            // 근거리 몬스터 공격
        }
        else if (Type == MonsterType.Ranged)
        {
            if (ShooterPrefab == null) return;

            Vector2 direction = DirectionToTarget();

            GameObject shot = Instantiate(ShooterPrefab, transform.position, Quaternion.identity);
            shot.transform.parent = transform.parent;

            Rigidbody2D _rigidbody = shot.GetComponent<Rigidbody2D>();
            _rigidbody.AddRelativeForce(direction * shootSpeed * 10f, ForceMode2D.Impulse);
            Debug.Log("Attack : " + _rigidbody.velocity);
        }
    }
    protected override void Movement(Vector2 direction)
    {
        direction = direction * speed;
        _rigidbody.velocity = direction;
    }


    /*내부 로직*/
    //=======================================//

    protected float DistanceToTarget() // Target(Player)
    {
        return Vector3.Distance(transform.position, target.position);

    }

    protected Vector2 DirectionToTarget()
    {
        return (target.position - transform.position).normalized;
    }
}