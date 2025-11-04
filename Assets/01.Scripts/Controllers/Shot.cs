using UnityEngine;

public class Shot : MonoBehaviour
{
    public float lifeTime = 10f;
    [SerializeField] int damage;

    //private void Start()
    //{
    //    Destroy(gameObject, lifeTime);
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 데미지 처리
            collision.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
