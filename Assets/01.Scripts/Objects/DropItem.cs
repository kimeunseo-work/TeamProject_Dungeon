using UnityEngine;

[System.Serializable]
public struct DropItemInfo
{
    public string Name;
    public int Amount;
}

public class DropItem : MonoBehaviour
{
    /*필드 & 프로퍼티*/
    //=======================================//

    // 플레이어 참조
    private Player targetData;
    private Transform targetPos;

    // 아이템 인포 참조
    [SerializeField] private DropItemInfo info;

    public string Name
    {
        get => info.Name;
        private set => info.Name = value;
    }
    public int Amount
    {
        get => info.Amount;
        private set => info.Amount = value;
    }

    // 스테이지 클리어시 true
    [SerializeField][Range(0, 100)] private float speed = 1f;
    [SerializeField][Range(0, 100)] private float acceleration = 1f;

    // 드랍 연출용
    public float moveDistance = 1f;   // 흩뿌릴 거리
    public float curveHeight = 0.5f;  // 곡선 높이
    public float duration = 0.5f;     // 이동 시간

    private bool canMove = false;
    private float timer = 0f;
    private const float cool = 0.2f;

    /*생명 주기*/
    //=======================================//

    private void Awake()
    {
        var go = GameObject.FindWithTag("Player");
        targetData = go.GetComponent<Player>();
        targetPos = go.GetComponent<Transform>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnDungeonSceneUnloaded += Instance_OnDungeonSceneUnloaded;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnDungeonSceneUnloaded -= Instance_OnDungeonSceneUnloaded;
    }

    private void Update()
    {
        if (timer < cool)
        {
            timer += Time.deltaTime;
        }
        else
        {
            canMove = true;
        }

        if (canMove) Movement();
    }

    /*events*/
    //=======================================//

    private void Instance_OnDungeonSceneUnloaded() => Release();

    /*충돌 & 트리거*/
    //=======================================//

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canMove) return;

        if (collision.CompareTag("Player"))
        {
            targetData.GetDungeonExp(Amount);

            Release();
        }
    }

    /*내부 로직*/
    //=======================================//

    private void Movement()
    {
        // 속도 증가
        speed += acceleration * Time.deltaTime;

        if (targetData == null || targetPos == null)
        {
            var go = GameObject.FindWithTag("Player");
            targetData = go.GetComponent<Player>();
            targetPos = go.GetComponent<Transform>();
        }

        // 방향 도출
        Vector2 direction = (targetPos.position - transform.position).normalized;
        // 이동
        transform.position += (Vector3)(speed * Time.deltaTime * direction);
    }

    private void Release()
    {
        // 초기 값으로
        speed = 0f;
        canMove = false;

        targetData.GetDungeonExp(1);
        ObjectManager.Instance.ExpPool.Release(gameObject);
    }
}