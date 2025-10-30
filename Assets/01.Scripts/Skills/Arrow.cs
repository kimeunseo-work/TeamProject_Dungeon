using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    public int pierceCount = 0; // 관통 수

    void Start()
    {
        Rigidbody2D rd = GetComponent<Rigidbody2D>();
        if(rd != null)
        {
            rd.velocity = transform.up * speed;
        }
        Destroy(gameObject, lifetime);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 나중에 데미지 적용 담당자가 여기서 처리
        // if (collision.CompareTag("Enemy")) Destroy(gameObject);
        //관통 처리
        if (pierceCount > 0)
        {
            pierceCount--;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
