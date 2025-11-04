using UnityEngine;
using UnityEngine.Pool;

public class ArrowPool : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private readonly int defaultCapacity = 5;
    [SerializeField] private readonly int maxSize = 10;
    private ObjectPool<GameObject> pool;

    void Awake()
    {
        pool = new ObjectPool<GameObject>(
            () => Instantiate(arrowPrefab),
            bullet => bullet.SetActive(true),
            //actionOnGet: null,
            bullet => bullet.SetActive(false),
            bullet => Destroy(bullet),
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    public GameObject Get() => pool.Get();
    public void Release(GameObject obj)
    {
        obj.transform.parent = gameObject.transform;
        pool.Release(obj);
    }
}
