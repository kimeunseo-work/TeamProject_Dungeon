using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseController : MonoBehaviour
{
    /*필드 & 프로퍼티*/
    //=======================================//

    protected Rigidbody2D _rigidbody;
    private SpriteRenderer charRenderer;
    protected AnimationHandler animationHandler;

    //private Transform WeaponPivot;
    //[SerializeField] private SpriteRenderer CharRenderer;
    //[SerializeField] private Transform WeaponPivot;
    [SerializeField] private Transform weaponTransform; // 무기(활) 오브젝트
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private Vector2 weaponOffset = new Vector2(0.5f, 0f);
    private Vector2 lastMoveDirection = Vector2.right;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get { return movementDirection; } }

    protected Vector2 knockback = Vector2.zero;
    protected float knockbackDuration = 0f;

    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get { return lookDirection; } }

    protected float Speed = 5f;
    public event Action<bool> OnMoveChanged;
    public bool IsMove { get; protected set; } = false;

    /*생명 주기*/
    //=======================================//

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        charRenderer = GetComponentInChildren<SpriteRenderer>();
        animationHandler = GetComponent<AnimationHandler>();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        Rotate(movementDirection);
    }

    protected virtual void FixedUpdate()
    {
        Movement(movementDirection);

        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime;
        }
    }

    /*외부 호출용*/
    //=======================================//
    public virtual void HandleAction()
    {
    }
    public virtual void TakeDamage()
    {
        animationHandler.Damage();
    }

    public virtual void Dead()
    {
        animationHandler.Dead();
    }

    /*내부 로직*/
    //=======================================//

    protected virtual void Movement(Vector2 direction)
    {
        // 움직이지 않을 때
        if (!IsMove) direction = default;

        direction = direction * Speed;

        if (knockbackDuration > 0.0f)
        {
            direction *= 0.2f;
            direction += knockback;
        }

        _rigidbody.velocity = direction;

        animationHandler.Move(direction);
    }

    protected virtual void Rotate(Vector2 direction)
    {
        // 캐릭터 이동 방향 저장 (사용하지 않더라도 유지)
        if (IsMove && direction != Vector2.zero)
        {
            lastMoveDirection = direction.normalized;
        }

        // 좌우 방향 판단 (애니메이션용)
        float rotZ = Mathf.Atan2(lastMoveDirection.y, lastMoveDirection.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90;
        charRenderer.flipX = isLeft;

        if (weaponTransform != null)
        {
            Transform target = FindNearestEnemy();
            if (target != null)
            {
                // 적 방향 계산
                Vector2 enemyDirection = (target.position - transform.position).normalized;
                float angleRad = Mathf.Atan2(enemyDirection.y, enemyDirection.x);
                float angleDeg = angleRad * Mathf.Rad2Deg;

                // 회전 반지름 설정 (캐릭터보다 큰 원)
                float radius = 1.0f; // 필요에 따라 조절

                // 원 위의 위치 계산
                Vector2 offset = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * radius;
                weaponTransform.position = transform.position + (Vector3)offset;

                // 무기 회전 (적 방향)
                weaponTransform.rotation = Quaternion.Euler(0f, 0f, angleDeg);
            }
        }
    }

    private Transform FindNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = hit.transform;
                }
            }
        }
        return nearest;
    }

    protected void CheckIsMoveChanged(Vector2 direction)
    {
        var currentMove = direction != default;
        if (IsMove != currentMove)
        {
            IsMove = currentMove;
            OnMoveChanged?.Invoke(IsMove);
        }
    }
}