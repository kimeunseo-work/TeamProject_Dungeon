using UnityEngine;

public class Player : Character
{
    private Monster target;

    private PlayerStatus status;
    private PlayerController controller;

    /*생명 주기*/
    //=======================================//

    private void Awake()
    {
        // 플레이어 데이터
        // targetTransform = 가장 가까운 몬스터

        // 플레이어 기능 스크립트 참조
        status = GetComponent<PlayerStatus>();
        controller = GetComponent<PlayerController>();

        // 스탯 초기화
        status.InitDungeon();
    }

    private void OnEnable()
    {
        status.OnDead += Status_OnDead;
    }
    private void OnDisable()
    {
        status.OnDead -= Status_OnDead;
    }

    /*외부 호출*/
    //=======================================//

    public override void TakeDamage(int amount)
    {
        // 데이터
        status.TakeDamage(amount);
        // 피격 액션
        //controller.TakeDamage(amount);
    }

    /*내부 로직*/
    //=======================================//

    protected override void Attack()
    {

    }

    /*이벤트 구독*/
    //=======================================//

    protected override void Status_OnDead()
    {
        // 사망 액션
        //controller.Dead(amount);
        
        // 삭제(나중에 시간되면 오브젝트 풀링 사용?)
        Destroy(gameObject);
    }
}