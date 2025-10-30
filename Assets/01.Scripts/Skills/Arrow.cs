using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float lifetime = 2f;
    public int pierceCount = 0; // 관통 수

    void Start()
    {
        Destroy(gameObject, lifetime);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 나중에 데미지 적용 담당자가 여기서 처리
        // if (collision.CompareTag("Enemy")) Destroy(gameObject);
        //관통 처리
        if (collision.CompareTag("Enemy"))
        {
            if (pierceCount > 0)
            {
                pierceCount--;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
