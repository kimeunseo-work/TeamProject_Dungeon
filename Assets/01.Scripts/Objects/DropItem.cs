using Unity.VisualScripting;
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
    [SerializeField] private bool canMove = false;
    [SerializeField][Range(0, 100)] private float speed = 1f;
    [SerializeField][Range(0, 100)] private float acceleration = 1f;

    // 드랍 연출용
    public float moveDistance = 1f;   // 흩뿌릴 거리
    public float curveHeight = 0.5f;  // 곡선 높이
    public float duration = 0.5f;     // 이동 시간

    private Vector3 startPos;
    private Vector3 endPos;

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
        // 스테이지 클리어 이벤트와 Stage_OnCompleted 구독
    }
    private void OnDisable()
    {
        // 스테이지 클리어 이벤트와 Stage_OnCompleted 해지
    }
    private void Update()
    {
        if (canMove) Movement();
    }

    /*초기화 전용*/
    //=======================================//

    public void Get(Vector2 monPos)
    {
        ObjectManager.Instance.ExpPool.Get();

        startPos = monPos;
        endPos = monPos;
    }

    /*이벤트 전용*/
    //=======================================//

    /// <summary>
    /// 스테이지 클리어 이벤트에 연결
    /// </summary>
    private void Stage_OnCompleted() => canMove = true;

    /*충돌 & 트리거*/
    //=======================================//

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
        // 방향 도출
        Vector2 dir = (targetPos.position - transform.position).normalized;
        // 이동
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }

    private void Release()
    {
        // 초기 값으로
        canMove = false;

        ObjectManager.Instance.ExpPool.Release(gameObject);
    }
}