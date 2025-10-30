using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float lifetime = 5f;
    public bool destroyOnHit = true;
    public int pierceCount = 0;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 나중에 데미지 적용 담당자가 여기서 처리
        // if (collision.CompareTag("Enemy")) Destroy(gameObject);
        //관통 처리
        if (!destroyOnHit) return;

        if (collision.CompareTag("Enemy") || collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
