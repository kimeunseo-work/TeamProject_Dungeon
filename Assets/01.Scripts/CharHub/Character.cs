using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Transform targetTransform; // 플레이어
    public bool CanAttack { get; protected set; }

    /*생명 주기*/
    //=======================================//

    protected virtual void Update()
    {
        if (CanAttack)
        {
            Attack();
            Debug.Log(CanAttack);
        }
    }

    /*외부 호출*/
    //=======================================//
    public abstract void TakeDamage(int amount);
    protected abstract void Attack();
    protected abstract void Status_OnDead();
}
