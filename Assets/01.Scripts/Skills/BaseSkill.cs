using UnityEngine;

public class BaseSkill : MonoBehaviour
{
    [SerializeField] SkillData skill;
    public string SkillName {  get; private set; }
    public float CoolDown { get;  set; }
    public float Timer { get; private set; } = 0f;
    public float ArrowSpeed { get; private set; }
    public int ArrowCount { get;  set; }
    public int ExtraPierce { get;  set; }
    public float SpreadAngle { get; private set; }
    public float ShotInterval { get; private set; }
    public bool CanPierce { get; private set; }
    public bool IsReady { get; private set; } = true;
    
    public void Init()
    {
        SkillName = skill.SkillName;
        CoolDown = skill.CoolDown;
        Timer = skill.Timer;
        ArrowSpeed = skill.ArrowSpeed;
        ArrowCount = skill.ArrowCount;
        ExtraPierce = skill.ExtraPierce;
        SpreadAngle = skill.SpreadAngle;
        ShotInterval = skill.ShotInterval;
        CanPierce = skill.CanPierce;
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if (IsReady) return;

        Timer += Time.deltaTime;

        if (Timer >= CoolDown)
            IsReady = true;
    }

    //public virtual void Activate(Transform launchPos, Transform direction)
    //{
    //    if (!IsReady) return;

    //    // 화살 여러 발 퍼뜨리기
    //    float angleStep = (ArrowCount > 1) ? SpreadAngle / (ArrowCount - 1) : 0f;
    //    float startAngle = -SpreadAngle / 2;

    //    for (int i = 0; i < ArrowCount; i++)
    //    {
    //        // 회전값 적용
    //        float angleOffset = startAngle + angleStep * i;
    //        Quaternion rotation = Quaternion.FromToRotation(Vector2.up, direction.position) * Quaternion.Euler(0, 0, angleOffset);

    //        // 플레이어 위치 기준 앞쪽 발사
    //        Vector2 spawnPos = launchPos.position + (direction.position * 0.5f);

    //        var arrowPrefab = ObjectManager.Instance.ArrowPool.Get();
    //        if (arrowPrefab == null)
    //        {
    //            Debug.LogWarning("arrowPrefab == null");
    //            return;
    //        }
    //        arrowPrefab.transform.SetPositionAndRotation(spawnPos, rotation);

    //        // 화살 속도 적용
    //        if (!arrowPrefab.TryGetComponent<Rigidbody2D>(out var rb))
    //        {
    //            Debug.Log("[SkillLaunch] Rigidbody2D is NULL. Attached script <Arrow>");
    //            rb = arrowPrefab.AddComponent<Rigidbody2D>();
    //        }
    //        rb.velocity =/* rotation * */direction.position * ArrowSpeed;

    //        // 관통 수 적용
    //        if (!arrowPrefab.TryGetComponent<Arrow>(out var arrowScript))
    //        {
    //            Debug.Log("[SkillLaunch] script <Arrow> is NULL. Attached script <Arrow>");
    //            arrowScript = arrowPrefab.AddComponent<Arrow>();
    //        }

    //        arrowScript.Init(CanPierce, ExtraPierce);
    //    }
    //}

    public virtual void Activate(Transform launchPos, Transform targetTransform)
    {
        if (!IsReady) return;
        IsReady = false;
        Timer = 0f;

        // 방향 벡터 계산
        Vector2 dir = ((Vector2)targetTransform.position - (Vector2)launchPos.position).normalized;

        // 화살 여러 발 퍼뜨리기
        float angleStep = (ArrowCount > 1) ? SpreadAngle / (ArrowCount - 1) : 0f;
        float startAngle = -SpreadAngle / 2;

        for (int i = 0; i < ArrowCount; i++)
        {
            float angleOffset = startAngle + angleStep * i;
            Quaternion rotation = Quaternion.Euler(0, 0, angleOffset);

            // 3발사 위치
            Vector2 spawnPos = (Vector2)launchPos.position + dir * 0.5f;

            var arrowObj = ObjectManager.Instance.ArrowPool.Get();
            if (arrowObj == null)
            {
                Debug.LogWarning("arrowPrefab == null");
                return;
            }

            arrowObj.transform.SetPositionAndRotation(spawnPos, Quaternion.LookRotation(Vector3.forward, dir));

            // 4️⃣ Rigidbody 설정
            if (!arrowObj.TryGetComponent<Rigidbody2D>(out var rb))
                rb = arrowObj.AddComponent<Rigidbody2D>();

            rb.isKinematic = true;
            rb.gravityScale = 0;
            rb.freezeRotation = true;

            // 5속도 적용
            rb.velocity = rotation * dir * ArrowSpeed;

            // Arrow 초기화
            if (!arrowObj.TryGetComponent<Arrow>(out var arrowScript))
                arrowScript = arrowObj.AddComponent<Arrow>();

            arrowScript.Init(CanPierce, ExtraPierce);
        }
    }
}