using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoArrowSkill : Skill
{
    [Header("투사체 설정")]
    public GameObject arrowPrefab;
    public float arrowSpeed = 10f; // 화살 속도

    public override void Activate()
    {
        if (arrowPrefab == null) return;

        // 화살 생성
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);

        // 화살에 속도 부여
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.right * arrowSpeed; // 오른쪽 방향으로 발사
        }

        Debug.Log($"{skillName} activated: Arrow fired!");
    }
}
