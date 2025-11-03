using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float delay = 2f;        // 폭발까지 시간
    public float explosionRadius = 2f;       // 폭발 범위
    public int damage = 30;
    public LayerMask targetLayer;   // Player 레이어 지정

    public GameObject rangeIndicator;      // 빨간 범위 표시 프리팹

    private GameObject rangeInstance;

    private void Start()
    {
        // 범위 표시 생성
        if (rangeIndicator != null)
        {
            rangeInstance = Instantiate(rangeIndicator, transform.position, Quaternion.identity, transform);
            rangeInstance.transform.localScale = Vector3.one * explosionRadius * 2f;
            // *2 : 반지름 -> 지름
        }

        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        Explode();
    }

    private void Explode()
    {
        // 폭발 범위 감지
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayer);
        foreach (var hit in hits)
        {
            var player = hit.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }

        // 범위 표시 삭제
        if (rangeInstance != null)
            Destroy(rangeInstance);

        // 폭탄 삭제
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
