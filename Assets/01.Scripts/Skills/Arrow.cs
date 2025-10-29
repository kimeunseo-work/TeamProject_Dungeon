using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 나중에 데미지 적용 담당자가 여기서 처리
        // 예시: if (collision.CompareTag("Enemy")) Destroy(gameObject);
        Destroy(gameObject);
    }
}
