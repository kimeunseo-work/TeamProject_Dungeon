using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Character
{
    /*필드 & 프로퍼티*/
    //=======================================//

    private Player target;

    [Header("몬스터 스탯")]
    [SerializeField] private int level;
    [SerializeField] private int attackSpeed;
    [SerializeField] private Status baseStatus;

    private float timer;

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

    private void Update()
    {
        if (!CanAttack)
        {
            timer = Time.deltaTime;

            if(timer >= attackSpeed)
            {
                CanAttack = true;
            }
        }

        if (CanAttack && TargetTransform != null 
            && controller.Type == MonsterController.MonsterType.Melee)
        {
            Attack();
        }

        controller.HandleAction();
    }

    /*외부 호출*/
    //=======================================//

    public override void TakeDamage(int amount)
    {
        status.TakeDamage(amount);

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
        if (!CanAttack) return;

        target.TakeDamage(status.DungeonAtk);
        CanAttack = false;
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