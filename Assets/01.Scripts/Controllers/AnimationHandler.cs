using UnityEngine;

public class AnimationHandler : MonoBehaviour 
{
    private static readonly int IsMoving = Animator.StringToHash("IsMove");
    private static readonly int IsDamage = Animator.StringToHash("IsDamage");
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Move(Vector2 obj)
    {
        bool ismoving = obj.magnitude > 0.1f;
        animator.SetBool(IsMoving, ismoving);
    }

    void Damage()
    {
        animator.SetBool(IsDamage, true);
    }
    void InvincibilityEnd()
    {
        animator.SetBool(IsDamage, false);
    }

    void Dead()
    {
        animator.SetBool(IsDead, true);
    }
}
