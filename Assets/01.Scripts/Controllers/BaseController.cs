using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BaseController : MonoBehaviour
{
    public Transform target;

    [SerializeField] private SpriteRenderer CharRenderer;
    [SerializeField] private Transform WeaponPivot;

    protected Rigidbody2D _rigidbody;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get { return movementDirection; } }

    protected Vector2 knockback = Vector2.zero;
    protected float  knockbackDuration = 0f;

    protected AnimationHandler animationHandler;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        CharRenderer = GetComponentInChildren<SpriteRenderer>();
        animationHandler = GetComponent<AnimationHandler>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleAction();
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

    protected virtual void HandleAction()
    {

    }

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

        CharRenderer.flipX = isLeft;
    }

    protected virtual void Dead()
    {

    }

}
