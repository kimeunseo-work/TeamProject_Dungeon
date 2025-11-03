using System;
using UnityEngine;

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
        if (!IsMove) direction = default;

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90;

        charRenderer.flipX = isLeft;
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