using UnityEngine;

public class Arrow : MonoBehaviour
{
    //private float lifetime = 5f;
    private bool canPierce = false;
    private int pierceCount = 0;

    //void Start()
    //{
    //    //Destroy(gameObject, lifetime);

    //    //void Launch(Vector3 direction, float speed)
    //    //{
    //    //    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    //    //    if (rb != null)
    //    //    {
    //    //        rb.velocity = direction.normalized * speed;
    //    //    }

    //    //    // 화살 이미지가 화살촉이 앞으로 향하도록
    //    //    transform.up = direction.normalized;

    //    //    // Rigidbody 회전 고정
    //    //    if (rb != null) rb.freezeRotation = true;
    //    //}
    //}

    private void OnEnable()
    {
        transform.rotation = default;
    }

    public void Init(bool canPierce, int pierceCount)
    {
        this.canPierce = canPierce;
        this.pierceCount = pierceCount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 이미 release를 진행한 경우 콜라이더 충돌 무시
        if (gameObject.activeSelf == false) return;

        if (collision.CompareTag("Player")) return;
        else if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Monster>().TakeDamage(10); // 하드 코딩

            // 관통 옵션
            if (canPierce && pierceCount > 0)
            {
                pierceCount--;
                return;
            }
        }

        // 몬스터와 충돌하지 않았다는 뜻은 벽에 부딪혔다는 뜻
        ObjectManager.Instance.ArrowPool.Release(gameObject);

        // 나중에 데미지 적용 담당자가 여기서 처리
        //if (collision.CompareTag("Enemy"))
        //{
        //    collision.GetComponent<Monster>().TakeDamage(10);
        //}
        //// 관통
        //if (!destroyOnHit) return;
    }
}