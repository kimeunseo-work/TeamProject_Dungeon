using System.Collections;
using UnityEngine;

public class BossMonster : Character
{
    private MonsterStatus status;   // 체력, 공격력 관리
    private Player target;
    private Rigidbody2D rb;

    [Header("공격 쿨타임")]
    public float patternCooldown = 2f;
    private float patternTimer = 0f;

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

    [Header("탄막 패턴")]
    public GameObject bulletPrefab;     // 탄막용 투사체 프리팹
    public int bulletCount = 12;        // 발사할 탄 수
    public float bulletSpeed = 6f;      // 탄속
    public float spreadAngle = 120f;    // 퍼지는 각도

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        status = GetComponent<MonsterStatus>();

        // 스탯 초기화
        var baseStatus = new Status { Hp = 500, Atk = 50 };  // 예시 값
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

        // 패턴 쿨타임 관리
        if (!isJumping)
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
        int pattern = Random.Range(0, 2); // 0: 점프, 1: 탄막
        switch (pattern)
        {
            case 0:
                StartCoroutine(JumpAttack());
                break;
            case 1:
                StartCoroutine(BarrageAttack());
                break;
        }
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

    private IEnumerator BarrageAttack()
    {
        Debug.Log("보스 패턴: 탄막 발사");

        if (bulletPrefab == null || target == null)
        {
            Debug.LogWarning("bulletPrefab 또는 target이 설정되지 않음!");
            yield break;
        }

        Vector2 directionToPlayer = (target.transform.position - transform.position).normalized;
        float baseAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        // 탄막 발사
        for (int i = 0; i < bulletCount; i++)
        {
            float angleOffset = -spreadAngle / 2 + (spreadAngle / (bulletCount - 1)) * i;
            float fireAngle = baseAngle + angleOffset;

            Quaternion rot = Quaternion.Euler(0, 0, fireAngle);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, rot);
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();

            if (rbBullet != null)
                rbBullet.AddForce(rot * Vector2.right * bulletSpeed, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(0.5f);
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
            return;

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
