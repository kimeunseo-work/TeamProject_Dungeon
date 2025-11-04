using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Bomb : MonoBehaviour
{
    [Header("폭발 설정")]
    public float delay = 2f;               // 폭발까지 시간
    public float explosionRadius = 2f;     // 폭발 범위
    public int damage = 30;                // 폭발 데미지
    public LayerMask targetLayer;          // Player 레이어 지정 (자동 세팅됨)

    [Header("이펙트 & 표시")]
    public GameObject rangeIndicator;      // 폭발 범위 시각화용 (빨간 원)
    public GameObject explosionEffect;     // 폭발 이펙트 (선택사항)

    private GameObject rangeInstance;
    private bool hasExploded = false;

    private void Start()
    {
        // 플레이어 레이어 자동 설정 (비워둬도 동작)
        if (targetLayer == 0)
            targetLayer = LayerMask.GetMask("Player");

        // 폭발 범위 표시 (시각 경고)
        if (rangeIndicator != null)
        {
            rangeInstance = Instantiate(rangeIndicator, transform.position, Quaternion.identity, transform);
            rangeInstance.transform.localScale = Vector3.one * explosionRadius * 2f;
        }

        // 폭발 대기 코루틴 실행
        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        // 폭발 직전 약간의 깜빡임 효과 (경고 느낌)
        float blinkTime = delay * 0.8f;
        float blinkSpeed = 0.2f;
        float elapsed = 0f;

        SpriteRenderer indicatorRenderer = rangeInstance?.GetComponent<SpriteRenderer>();
        if (indicatorRenderer != null)
        {
            while (elapsed < blinkTime)
            {
                indicatorRenderer.enabled = !indicatorRenderer.enabled;
                yield return new WaitForSeconds(blinkSpeed);
                elapsed += blinkSpeed;
            }
            indicatorRenderer.enabled = true;
        }

        // 폭발 실행
        yield return new WaitForSeconds(delay - blinkTime);
        Explode();
    }
    
    private void ExplodeEffect()
    {
        GameObject ps = Instantiate(
                explosionEffect,
                transform.position,   // 현재 총알 위치
                Quaternion.identity   // 회전 없음
            );

        Destroy(ps, 2f);
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // 폭발 이펙트 생성
        if (explosionEffect != null)
            ExplodeEffect();

        // 폭발 범위 감지
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayer);
        foreach (var hit in hits)
        {
            Player player = hit.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log($"폭탄 데미지 {damage} 적용됨 -> {player.name}");
            }
        }

        // 범위 표시 제거
        if (rangeInstance != null)
            Destroy(rangeInstance);
        // 폭탄 자체 제거
        Destroy(gameObject);
    }
}
