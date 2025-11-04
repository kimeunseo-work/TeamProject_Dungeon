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
    //public float ShotInterval { get; private set; }
    public bool CanPierce { get; private set; }
    public bool IsReady { get; private set; } = true;

    private void Awake()
    {
        if(skill == null)
        {
            skill = Resources.Load<SkillData>("defaultSkill");
        }
    }

    public void Init()
    {
        SkillName = skill.SkillName;
        CoolDown = skill.CoolDown;
        Timer = skill.Timer;
        ArrowSpeed = skill.ArrowSpeed;
        ArrowCount = skill.ArrowCount;
        ExtraPierce = skill.ExtraPierce;
        SpreadAngle = skill.SpreadAngle;
        //ShotInterval = skill.ShotInterval;
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

    public virtual void Activate(Transform launchPos, Transform targetTransform, int damage, float speed)
    {
        if (!IsReady) return;
        IsReady = false;
        Timer = 0f;
        AudioManager.instance.ArrowShot();
        // 기본 발사 방향
        Vector2 dir = ((Vector2)targetTransform.position - (Vector2)launchPos.position).normalized;

        for (int i = 0; i < ArrowCount; i++)
        {
            // 균등한 각도 분포 계산 (-SpreadAngle/2 ~ +SpreadAngle/2)
            float t = (ArrowCount == 1) ? 0.5f : (float)i / (ArrowCount - 1);
            float angleOffset = Mathf.Lerp(-SpreadAngle / 2f, SpreadAngle / 2f, t);

            // dir 기준으로 angleOffset만큼 회전한 실제 발사 방향
            Vector2 rotatedDir = Quaternion.AngleAxis(angleOffset, Vector3.forward) * dir;

            // 모든 화살은 같은 시작 위치에서 나감
            Vector2 spawnPos = (Vector2)launchPos.position;

            // 풀에서 화살 꺼내기
            var arrowObj = ObjectManager.Instance.ArrowPool.Get();
            if (arrowObj == null)
            {
                Debug.LogWarning("arrowPrefab == null");
                return;
            }

            // 회전: 화살이 자신이 날아갈 방향을 바라보게 설정
            arrowObj.transform.rotation = Quaternion.FromToRotation(Vector2.up, rotatedDir);

            // 위치 지정
            arrowObj.transform.position = spawnPos;

            // Rigidbody2D 설정
            if (!arrowObj.TryGetComponent<Rigidbody2D>(out var rb))
                rb = arrowObj.AddComponent<Rigidbody2D>();

            rb.isKinematic = true;
            rb.gravityScale = 0;
            rb.freezeRotation = true;
            
            // 실제 발사
            rb.velocity = rotatedDir * (ArrowSpeed + speed);

            // Arrow 초기화
            if (!arrowObj.TryGetComponent<Arrow>(out var arrowScript))
                arrowScript = arrowObj.AddComponent<Arrow>();

            arrowScript.Init(CanPierce, ExtraPierce, damage);
        }
    }

}