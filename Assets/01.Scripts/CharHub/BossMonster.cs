using System.Collections;
using UnityEngine;

public class BossMonster : Character
{
    private MonsterStatus status;   //체력, 공격력 등은 여기서 관리
    private Player target;
    private Rigidbody2D rb;

    [Header("공격 쿨타임")]
    public float patternCooldown = 2f;
    private float patternTimer = 0f;

    [Header("돌진 패턴")]
    public float dashSpeed = 10f;
    public float dashDuration = 1f;
    public int dashDamage = 20;
    private bool isDashing = false;

    [Header("점프 패턴")]
    public float jumpHeight = 5f;
    public float jumpDuration = 1f;
    public int jumpDamage = 50;
    private bool isJumping = false;

    [Header("폭탄 패턴")]
    public GameObject bombPrefab;
    public Vector2 bombSpawnRange = new Vector2(5f, 5f);
    public float bombCooldown = 5f;
    private float bombTimer = 0f;
    public float bombExplosionRadius = 2f;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        status = GetComponent<MonsterStatus>();

        //스탯 초기화 (Monster처럼)
        var baseStatus = new Status { Hp = 500, Atk = 50 };  // 또는 데이터에서 불러오기
        status.InitDungeon(baseStatus, 1);
    }

    protected override void Update()
    {
        base.Update();
        if (target == null) return;

        // 폭탄 쿨타임 관리
        bombTimer += Time.deltaTime;
        if (bombTimer >= bombCooldown)
        {
            SpawnBomb();
            bombTimer = 0f;
        }

        // 다른 패턴 쿨타임 관리
        if (!isDashing && !isJumping)
        {
            patternTimer += Time.deltaTime;
            if (patternTimer >= patternCooldown)
            {
                patternTimer = 0f;
                ChoosePattern();
            }
        }
    }

    private void ChoosePattern()
    {
        int pattern = Random.Range(0, 2); // 0: 돌진, 1: 점프
        switch (pattern)
        {
            case 0:
                StartCoroutine(DashAttack());
                break;
            case 1:
                StartCoroutine(JumpAttack());
                break;
        }
    }

    private IEnumerator DashAttack()
    {
        isDashing = true;
        float elapsed = 0f;
        Vector2 direction = (target.transform.position - transform.position).normalized;
        rb.velocity = direction * dashSpeed;

        while (elapsed < dashDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        isDashing = false;
    }

    private IEnumerator JumpAttack()
    {
        isJumping = true;
        Vector3 startPos = transform.position;
        Vector3 apex = startPos + Vector3.up * jumpHeight;

        float elapsed = 0f;
        while (elapsed < jumpDuration / 2f)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, apex, elapsed / (jumpDuration / 2f));
            yield return null;
        }

        elapsed = 0f;
        Vector3 targetPos = new Vector3(target.transform.position.x, startPos.y, startPos.z);
        while (elapsed < jumpDuration / 2f)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(apex, targetPos, elapsed / (jumpDuration / 2f));
            yield return null;
        }

        Collider2D hit = Physics2D.OverlapCircle(transform.position, 1.2f, LayerMask.GetMask("Player"));
        if (hit != null)
        {
            Player p = hit.GetComponent<Player>();
            if (p != null) p.TakeDamage(jumpDamage);
        }

        isJumping = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing && collision.collider.CompareTag("Player"))
        {
            Player targetPlayer = collision.collider.GetComponent<Player>();
            if (targetPlayer != null)
                targetPlayer.TakeDamage(dashDamage);
        }
    }

    private void SpawnBomb()
    {
        if (bombPrefab == null || target == null) return;

        Vector3 spawnPos = target.transform.position + new Vector3(
            Random.Range(-bombSpawnRange.x, bombSpawnRange.x),
            Random.Range(-bombSpawnRange.y, bombSpawnRange.y),
            0f
        );

        GameObject bomb = Instantiate(bombPrefab, spawnPos, Quaternion.identity);
        Bomb bombScript = bomb.GetComponent<Bomb>();
        if (bombScript != null)
            bombScript.explosionRadius = bombExplosionRadius;
    }

    public override void TakeDamage(int amount)
    {
        if (status == null)
        {
            return;
        }

        status.TakeDamage(amount);


        if (status.DungeonHp <= 0 && !status.IsDead)
        {
            Status_OnDead();
        }
    }

    private void OnEnable()
    {
        if (status != null)
            status.OnDead += Status_OnDead;
    }

    private void OnDisable()
    {
        if (status != null)
            status.OnDead -= Status_OnDead;
    }

    protected override void Status_OnDead()
    {
        Debug.Log("보스 사망!");
        Destroy(gameObject);
    }

    protected override void Attack() { }
}
