using UnityEngine;
public class AutoArrowSkill : Skill
{
    [Header("화살 투사체 설정")]
    public GameObject arrowPrefab;
    public float arrowSpeed = 10f;        // 화살 속도
    public float detectionRadius = 10f;   // 적 감지 반경
    public int arrowCount = 1;            // 한 번에 발사할 화살 수
    public int extraPierce = 0;           // 추가 관통 수
    public float spreadAngle = 0f;        // 화살 퍼짐 각도
    public float shotInterval = 0.1f;     // 발사 쿨타임

    private bool canShoot = true;

    public override void Activate()
    {
        if (!canShoot || arrowPrefab == null) return;
        arrowPrefab = GetArrow();
        if (arrowPrefab == null)
        {
            Debug.LogWarning("arrowPrefab이 설정되지 않았습니다.");
            return;
        }

        canShoot = false;
        FireArrows();
        StartCoroutine(ResetShotCooldown());
    }

    private void FireArrows()
    {
        // 가장 가까운 적 찾기
        Transform target = FindNearestEnemy();

        // 발사 기준 방향
        Vector2 direction = target != null
            ? (target.position - transform.position).normalized
            : transform.up; // 적 없으면 플레이어 위쪽

        // 화살 여러 발 퍼뜨리기
        float angleStep = (arrowCount > 1) ? spreadAngle / (arrowCount - 1) : 0f;
        float startAngle = -spreadAngle / 2;

        for (int i = 0; i < arrowCount; i++)
        {
            float angleOffset = startAngle + angleStep * i;

            // 회전값 적용
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, direction) * Quaternion.Euler(0, 0, angleOffset);

            // 플레이어 위치 기준 앞쪽 발사
            Vector3 spawnPos = transform.position + (Vector3)(direction * 0.5f);
            GameObject arrow = Instantiate(arrowPrefab, spawnPos, rotation);

            // 화살 속도 적용
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = rotation * Vector3.up * arrowSpeed;
            }

            // 관통 수 적용
            Arrow arrowScript = arrow.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                arrowScript.pierceCount += extraPierce;
            }
        }

        Debug.Log($"{skillName} fired {arrowCount} arrows with {extraPierce} extra pierce.");
    }

    private IEnumerator ResetShotCooldown()
    {
        yield return new WaitForSeconds(shotInterval);
        canShoot = true;
    }

    private Transform FindNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = hit.transform;
                }
            }
        }
        return nearest;
    }

    protected GameObject GetArrow()
    {
        return ObjectManager.Instance.ArrowPool.Get();
    }
}
