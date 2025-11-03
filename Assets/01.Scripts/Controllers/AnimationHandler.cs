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
    bool HasParameter(int hash)
    {
        for (int i = 0; animator.parameterCount > 0; i++)
        {
            if(animator.GetParameter(i).nameHash == hash)
                return true;
        }
        return false;
    }

    public void Move(Vector2 obj)
    {
        if (!HasParameter(IsMoving)) return;
        bool ismoving = obj.magnitude > 0.1f;
        animator.SetBool(IsMoving, ismoving);
    }

    public void Damage()
    {
        if (!HasParameter(IsDamage)) return;
        animator.SetBool(IsDamage, true);
    }
    public void InvincibilityEnd()
    {
        animator.SetBool(IsDamage, false);
    }

    public void Dead()
    {
        if (!HasParameter(IsDead)) return;
        animator.SetBool(IsDead, true);
    }
}
