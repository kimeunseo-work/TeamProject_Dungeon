using UnityEngine;

public class Arrow : MonoBehaviour
{
    private const float lifetime = 5f;
    private float timer = 0f;
    private bool canPierce = false;
    private int pierceCount = 0;
    private const int defaultDamage = 5;
    private int damage = 0;

    public void Init(bool canPierce, int pierceCount, int damage)
    {
        timer = 0f;
        this.canPierce = canPierce;
        this.pierceCount = pierceCount;
        this.damage = defaultDamage + damage;
    }

    private void OnEnable()
    {
        transform.rotation = default;
    }

    private void Update()
    {
        if(timer < lifetime)
            timer += Time.deltaTime;
        else
            ObjectManager.Instance.ArrowPool.Release(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 이미 release를 진행한 경우 콜라이더 충돌 무시
        if (gameObject.activeSelf == false) return;

        if (collision.CompareTag("Enemy") || collision.CompareTag("Wall"))
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<Monster>().TakeDamage(damage); // 하드 코딩
                AudioManager.instance.ArrowHit();
                // 관통 옵션
                if (canPierce && pierceCount > 0)
                {
                    pierceCount--;
                    return;
                }
            }

            ObjectManager.Instance.ArrowPool.Release(gameObject);
        }
    }
}