using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitArrowSkill : Skill
{
    [Header("Orbit 화살 설정")]
    public GameObject orbitArrowPrefab; // 회전용 화살
    public GameObject shotArrowPrefab;  // 발사용 화살
    public int orbitCount = 3;          // 플레이어 주변 Orbit 화살 수
    public float orbitRadius = 1.5f;    // Orbit 반경
    public float orbitSpeed = 90f;      // 회전 속도 (도/초)
    public float shotInterval = 1.0f;   // 발사 간격
    public float shotSpeed = 10f;       // 발사 화살 속도

    private List<GameObject> orbitArrows = new List<GameObject>();
    private List<Vector3> orbitOffsets = new List<Vector3>();
    private Transform player;

    protected override void Start()
    {
        base.Start();
        player = transform; // 스킬이 붙은 플레이어
        InitializeOrbitArrows();
        StartCoroutine(AutoFireRoutine());
    }

    private void InitializeOrbitArrows()
    {
        orbitArrows.Clear();
        orbitOffsets.Clear();

        float angleStep = 360f / orbitCount;

        for (int i = 0; i < orbitCount; i++)
        {
            float angle = angleStep * i;
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * orbitRadius;
            orbitOffsets.Add(offset);

            GameObject arrow = Instantiate(orbitArrowPrefab, player.position + offset, Quaternion.identity);
            orbitArrows.Add(arrow);
        }
    }

    void Update()
    {
        if (orbitArrows.Count == 0) return;

        for (int i = 0; i < orbitArrows.Count; i++)
        {
            // null 체크: Destroy됐으면 다시 생성
            if (orbitArrows[i] == null)
            {
                GameObject arrow = Instantiate(orbitArrowPrefab, player.position + orbitOffsets[i], Quaternion.identity);
                orbitArrows[i] = arrow;
            }

            // 회전 계산
            float angle = orbitSpeed * Time.time + (360f / orbitCount) * i;
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * orbitRadius;

            // 위치 갱신
            orbitArrows[i].transform.position = player.position + offset;

            // 방향 설정 (중심을 바라보도록)
            orbitArrows[i].transform.up = (orbitArrows[i].transform.position - player.position).normalized;
        }
    }

    private IEnumerator AutoFireRoutine()
    {
        while (true)
        {
            foreach (var orbitArrow in orbitArrows)
            {
                if (orbitArrow != null)
                {
                    // orbit 위치에서 직선 발사
                    FireShotArrow(orbitArrow.transform.position, orbitArrow.transform.up);
                }
            }
            yield return new WaitForSeconds(shotInterval);
        }
    }

    private void FireShotArrow(Vector3 position, Vector3 direction)
    {
        if (shotArrowPrefab == null) return;

        GameObject arrow = Instantiate(shotArrowPrefab, position, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction.normalized * shotSpeed;
        }

        // 필요하면 관통 수 등 패시브 적용 가능
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.pierceCount = 0;
        }
    }

    public override void Activate()
    {
        // Activate 호출 시, AutoFireRoutine 시작
        // 이미 Start에서 코루틴 시작하므로 여기선 별도 처리 안 해도 됨
    }
}
