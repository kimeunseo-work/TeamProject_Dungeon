using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Shot : MonoBehaviour
{
    public float lifeTime = 10f;
    [SerializeField] int damage;
    [SerializeField] ParticleSystem particle;

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

            //gameObject.SetActive(false);
            SpawnParticle();
            Destroy(gameObject);
            return;
        }

        if (collision.CompareTag("Wall"))
        {
            //gameObject.SetActive(false);
            SpawnParticle();
            Destroy(gameObject);
            return;
        }
    }

    private void SpawnParticle()
    {
        if (particle != null)
        {
            ParticleSystem instance = Instantiate(
                particle,
                transform.position,   // 현재 총알 위치
                Quaternion.identity   // 회전 없음
            );

            instance.transform.parent = null;
            instance.Play();
            Destroy(instance.gameObject, instance.main.duration);
        }
    }
}
