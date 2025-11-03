using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Transform TargetTransform { get; protected set; } = null;
    public bool CanAttack { get; protected set; } = true;


    /*생명 주기*/
    //=======================================//

    /// <summary>
    /// Attack() 존재
    /// </summary>
    protected virtual void Update()
    {
        if (CanAttack && TargetTransform != null)
        {
            Attack();
        }
    }

    /*외부 호출*/
    //=======================================//
    public abstract void TakeDamage(int amount);
    protected abstract void Attack();
    protected abstract void Status_OnDead();
}
