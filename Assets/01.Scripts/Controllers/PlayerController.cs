using UnityEngine;

public class PlayerController : BaseController
{
    /*���� �ֱ�*/
    //=======================================//

    protected override void Start()
    {
        base.Start();
    }

    /*�ܺ� ȣ��*/
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
