using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Character
{
    /*필드 & 프로퍼티*/
    //=======================================//

    private Player target;

    // 나중에 데이터화
    [SerializeField] private int level;
    [SerializeField] private Status baseStatus;

    private MonsterStatus status;
    private MonsterController controller;

    [Header("Hp Bar")]
    [SerializeField] Slider hpSlider;
    [SerializeField] TextMeshProUGUI hpText;

    /*생명 주기*/
    //=======================================//

    private void Awake()
    {
        // 플레이어 데이터
        var go = GameObject.FindWithTag("Player");
        TargetTransform = go.GetComponent<Transform>();
        target = go.GetComponent<Player>();

        // 몬스터 기능 스크립트 참조
        status = GetComponent<MonsterStatus>();
        controller = GetComponent<MonsterController>();

        // 스탯 초기화 - 나중에 데이터로
        status.InitDungeon(baseStatus, level);
        controller.Init(TargetTransform);
        CanAttack = false;
    }

    private void OnEnable()
    {
        UpdateHpbar();
        status.OnDead += Status_OnDead;
    }
    private void OnDisable()
    {
        status.OnDead -= Status_OnDead;
        StageManager.Instance.OnMonsterKilled();

        SpreadExp(5);
    }

    protected override void Update()
    {
        base.Update();

        controller.HandleAction();
    }

    /*외부 호출*/
    //=======================================//

    public override void TakeDamage(int amount)
    {
        // 데이터
        status.TakeDamage(amount);
        // 피격 액션
        // controller.TakeDamage(amount);

        UpdateHpbar();
    }

    /*충돌&트리거*/
    //=======================================//

    /// <summary>
    /// 공격 위한 것
    /// </summary>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CanAttack = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CanAttack = false;
        }
    }

    /*내부 로직*/
    //=======================================//
    
    protected override void Attack()
    {
        // 내부 타이머 돌려서 공격 못하는 상태면 그냥 리턴
        // 데이터
        target.TakeDamage(status.DungeonAtk);
        // 공격 액션
        // controller.TakeDamage(amount);
    }

    private void SpreadExp(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            float angle = i * 360f / amount;
            Vector2 pos = transform.position + (Vector3)(Quaternion.Euler(0, 0, angle) * Vector2.up * 2f);

            var exp = ObjectManager.Instance.ExpPool.Get();// 오브젝트 Get
            exp.transform.position = pos;
            exp.transform.localRotation = Quaternion.identity;

            exp.transform.up = (transform.position - exp.transform.position).normalized; // 중심 바라보게 회전
        }
    }

    /*이벤트 구독*/
    //=======================================//
    
    protected override void Status_OnDead()
    {
        // 사망 액션
        controller.Dead();
        // 경험치 오브젝트 뿌리기

        // 삭제(나중에 시간되면 오브젝트 풀링 사용?)
        Destroy(gameObject);
    }

    private void UpdateHpbar()
    {
        float targetValue = (float)status.DungeonHp / status.DungeonMaxHp;
        hpText.text = status.DungeonHp.ToString();
        UIManager.Instance.AnimateSlider(hpSlider, targetValue);
    }
}