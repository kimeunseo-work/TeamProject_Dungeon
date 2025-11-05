using UnityEngine;
using UnityEngine.SceneManagement;

public class Arrow : MonoBehaviour
{
    private const float lifetime = 5f;
    private float timer = 0f;
    private bool canPierce = false;
    private int pierceCount = 0;
    private const int defaultDamage = 5;
    private int damage = 0;

    [SerializeField] ParticleSystem particle;

    private void Awake()
    {
        SceneManager.activeSceneChanged += SceneManager_sceneUnloaded;
    }

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
        if (timer < lifetime)
            timer += Time.deltaTime;
        else
            ObjectManager.Instance.ArrowPool.Release(gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= SceneManager_sceneUnloaded;
    }

    private void SceneManager_sceneUnloaded(Scene arg0, Scene arg1)
    {
        if (gameObject.activeSelf == false) return;
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

                AudioManager.Instance.ArrowHit();
                // 관통 옵션
                if (canPierce && pierceCount > 0)
                {
                    pierceCount--;
                    return;
                }
            }
            SpawnParticle();
            ObjectManager.Instance.ArrowPool.Release(gameObject);
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