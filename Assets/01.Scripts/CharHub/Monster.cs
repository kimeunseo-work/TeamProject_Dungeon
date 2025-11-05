using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Character
{
    /*필드 & 프로퍼티*/
    //=======================================//

    private Player target;

    // 나중에 데이터화
    //[SerializeField] private int level;
    [SerializeField] private Status baseStatus;

    private MonsterStatus status;
    private MonsterController controller;

    [Header("Hp Bar")]
    [SerializeField] Slider hpSlider;
    [SerializeField] TextMeshProUGUI hpText;

    private Coroutine sliderCoroutine;
    /*생명 주기*/
    //=======================================//

    private void OnDisable()
    {
        status.OnDead -= Status_OnDead;
        StageManager.Instance.OnMonsterKilled();
    }

    protected override void Update()
    {
        base.Update();

        controller.HandleAction();
    }

    /*외부 호출*/
    //=======================================//

    public void Init(int level)
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

        status.OnDead += Status_OnDead;

        UpdateHpbar();
    }

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
        target.TakeDamage(status.DungeonAtk);
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
        SpreadExp(5);
        // 삭제(나중에 시간되면 오브젝트 풀링 사용?)
        Destroy(gameObject);
    }

    private void UpdateHpbar()
    {
        float targetValue = (float)status.DungeonHp / status.DungeonMaxHp;
        hpText.text = status.DungeonHp.ToString();
        AnimateSlider(hpSlider, targetValue);
    }

    public void AnimateSlider(Slider slider, float targetValue, float duration = 0.4f)
    {
        if (slider == null) return;

        if (sliderCoroutine != null)
            StopCoroutine(sliderCoroutine);

        sliderCoroutine = StartCoroutine(SliderCoroutine(slider, targetValue, duration));
    }

    private IEnumerator SliderCoroutine(Slider slider, float targetValue, float duration)
    {
        float startValue = slider.value;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            slider.value = Mathf.Lerp(startValue, targetValue, t);
            yield return null;
        }

        slider.value = targetValue;
        sliderCoroutine = null;
    }
}