using UnityEngine;
using UnityEngine.Pool;

public class ExpPool : MonoBehaviour
{
    [SerializeField] private GameObject expPrefab;
    private ObjectPool<GameObject> pool;

    void Awake()
    {
        pool = new ObjectPool<GameObject>(
            () => Instantiate(expPrefab),
            bullet => bullet.SetActive(true),
            bullet => bullet.SetActive(false),
            bullet => Destroy(bullet),
            defaultCapacity: 3,
            maxSize: 5
        );
    }

    public GameObject Get() => pool.Get();
    public void Release(GameObject obj) => pool.Release(obj);
}