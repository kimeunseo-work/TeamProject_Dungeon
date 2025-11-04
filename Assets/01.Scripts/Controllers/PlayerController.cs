using UnityEngine;

public class PlayerController : BaseController
{
    /*외부 호출용*/
    //=======================================//

    public override void HandleAction()
    {
        float horizental = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector2(horizental, vertical).normalized;

        CheckIsMoveChanged(movementDirection);
    }
    //public override void Attack()
    //{
    //    base.Attack();
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            Debug.Log("Exit Triggered!");
            StageManager stageManager = FindObjectOfType<StageManager>();

            if (stageManager != null)
            {
                stageManager.GoToNextStage();
            }
        }
    }
}
