using UnityEngine;

public class PlayerController : BaseController
{
    /*생명 주기*/
    //=======================================//

    protected override void Start()
    {
        base.Start();
    }

    /*외부 호출용*/
    //=======================================//

    public override void HandleAction()
    {
        float horizental = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector2(horizental, vertical).normalized;

        CheckIsMoveChanged(movementDirection);
    }

    public override void Dead()
    {
        Time.timeScale = 0f;
    }
}
