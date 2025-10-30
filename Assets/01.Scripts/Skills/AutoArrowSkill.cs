using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoArrowSkill : Skill
{
    [Header("화살 투사체 설정")]
    public GameObject arrowPrefab;
    public float arrowSpeed = 10f; // 화살 속도

    public int arrowCount = 1; // 발사할 화살 개수
    public int extraPierce = 0; // 추가 관통 수
    public override void Activate()
    {
        if (arrowPrefab == null) return;

        float spreadAngle = 15f; // 화살 퍼짐 각도
        int count = arrowCount;
        float angleStep = 0f;

        if (count > 1)
        {
            angleStep = spreadAngle / (count - 1);
        }

        float startAngle = -spreadAngle / 2;

        for (int i = 0; i < arrowCount; i++)
        {
            // 각도 계산
            float angle = startAngle + (angleStep * i);
            Quaternion rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + angle);

            // 화살 생성
            Vector3 spawnOffset = transform.up * 1.0f;
            GameObject arrow = Instantiate(arrowPrefab, transform.position, rotation);

            // 화살에 속도 부여
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = rotation * transform.right * arrowSpeed; // 오른쪽 방향으로 발사
            }

            // 관통 수 설정
            Arrow arrowScript = arrow.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                arrowScript.pierceCount += extraPierce;
            }
        }

        Debug.Log($"{skillName} activated: Fired {arrowCount} arrows with {extraPierce} extra pierce.");
    }
}
