using System.Collections;
using System.Collections.Generic;
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
    protected float  knockbackDuration = 0f;

    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get { return lookDirection; } }


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
        //HandleAction();
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

    /*외부 호출*/
    //=======================================//
    public virtual void HandleAction()
    {
    }

    public virtual void Dead()
    {
    }

    /*내부 로직*/
    //=======================================//

    protected virtual void Movement(Vector2 direction)
    {
        direction = direction * 5;
        if (knockbackDuration > 0.0f)
        {
            direction *= 0.2f;
            direction += knockback;
        }

        _rigidbody.velocity = direction;
    }

    protected virtual void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90;

        charRenderer.flipX = isLeft;
    }
}