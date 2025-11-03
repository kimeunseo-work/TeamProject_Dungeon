using System.Collections;
using UnityEngine;

public class AnimationHandler : MonoBehaviour 
{
    private static readonly int IsMoving = Animator.StringToHash("IsMove");
    private static readonly int IsDamage = Animator.StringToHash("IsDamage");
    private static readonly int IsDead = Animator.StringToHash("IsDead");
    private static readonly int IsAttack = Animator.StringToHash("IsAttack");

    private bool isDamage = false;
    private bool isDead = false;
    private bool isAttack = false;

    float time;
    float destroyTime;

    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    bool HasParameter(int hash)
    {
        for (int i = 0; animator.parameterCount > 0; i++)
        {
            if(animator.GetParameter(i).nameHash == hash)
                return true;
        }
        return false;
    }

    private void Update()
    {
        if (isDamage)
        {
            time += Time.deltaTime;

            if (time > 0.5f)
            {
                isDamage = false;
                InvincibilityEnd();
                time = 0f;
            }
        }
        if (isDead)
        {
            destroyTime += Time.deltaTime;
            if (destroyTime > 0.8f)
            {
                isDead = false;
                animator.SetBool(IsDead, isDead);
                time = 0f;
                Destroy(gameObject);
            }
        }
    }

    public void Move(Vector2 obj)
    {
        if (!HasParameter(IsMoving)) return;
        bool ismoving = obj.magnitude > 0.1f;
        animator.SetBool(IsMoving, ismoving);
    }
    public void Attack()
    {
        if (!HasParameter(IsMoving)) return;
        isAttack = true;
        animator.SetBool(IsAttack, isAttack);
    }

    public void Damage()
    {
        if (!HasParameter(IsDamage)) return;
        isDamage = true;
        animator.SetBool(IsDamage, isDamage);
    }

    public void InvincibilityEnd()
    {
        animator.SetBool(IsDamage, false);
    }

    public void Dead()
    {
        if (!HasParameter(IsDead)) return;
        isDead = true;
        animator.SetBool(IsDead, isDead);
    }
}
