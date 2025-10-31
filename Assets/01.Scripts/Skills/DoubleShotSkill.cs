using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleShotSkill : AutoArrowSkill
{
    [Header("더블샷 설정")]
    public float secondShotDelay = 0.15f; // 두 번째 화살 딜레이

    protected override void Activate()
    {
        if (arrowPrefab == null) return;

        // 첫 번째 발사
        StartCoroutine(FireDoubleShot());
    }

    private IEnumerator FireDoubleShot()
    {
        // 첫 번째 화살 발사
        FireArrow();

        // 딜레이 후 두 번째 화살 발사
        yield return new WaitForSeconds(secondShotDelay);
        FireArrow();

        Debug.Log($"{skillName} activated: Double shot fired!");
    }

    private void FireArrow()
    {
        Quaternion rotation = transform.rotation;

        // 화살 생성
        Vector3 spawnOffset = transform.up * 0.5f;
        GameObject arrow = Instantiate(arrowPrefab, transform.position + spawnOffset, rotation);

        // 속도 부여
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.up * arrowSpeed;
        }

        // 관통 수 설정
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.pierceCount += extraPierce;
        }
    }
}
