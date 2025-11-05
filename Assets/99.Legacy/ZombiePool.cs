using UnityEngine;
using UnityEngine.Pool;

public class ZombiePool : MonoBehaviour 
{
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private readonly int defaultCapacity = 5;
    [SerializeField] private readonly int maxSize = 10;
    private ObjectPool<GameObject> pool;

    void Awake()
    {
        pool = new ObjectPool<GameObject>(
            () => Instantiate(zombiePrefab),
            bullet => bullet.SetActive(true),
            bullet => bullet.SetActive(false),
            bullet => Destroy(bullet),
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    public GameObject Get() => pool.Get();
    public void Release(GameObject obj) => pool.Release(obj);
}
