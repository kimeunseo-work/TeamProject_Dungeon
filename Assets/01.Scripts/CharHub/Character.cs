using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Transform TargetTransform { get; protected set; } = null;
    public bool CanAttack { get; protected set; } = true;

    /*외부 호출*/
    //=======================================//
    public abstract void TakeDamage(int amount);
    protected abstract void Attack();
    protected abstract void Status_OnDead();
}
