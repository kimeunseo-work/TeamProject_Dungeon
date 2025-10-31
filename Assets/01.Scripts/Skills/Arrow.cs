using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float lifetime = 5f;
    public bool destroyOnHit = true;
    public int pierceCount = 0;

    void Start()
    {
        Destroy(gameObject, lifetime);

        void Launch(Vector3 direction, float speed)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction.normalized * speed;
            }

            // 화살 이미지가 화살촉이 앞으로 향하도록
            transform.up = direction.normalized;

            // Rigidbody 회전 고정
            if (rb != null) rb.freezeRotation = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Monster>().TakeDamage(10);
        }

        // 관통 옵션
        if (!destroyOnHit) return;

        // 플레이어 
        if (collision.CompareTag("Enemy") || collision.CompareTag("Wall"))
        {
            ObjectManager.Instance.ArrowPool.Release(gameObject);
        }

        // 나중에 데미지 적용 담당자가 여기서 처리
        //if (collision.CompareTag("Enemy"))
        //{
        //    collision.GetComponent<Monster>().TakeDamage(10);
        //}
        //// 관통
        //if (!destroyOnHit) return;
    }
}