using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : Character
{
    private MonsterStatus status;
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
    public GameObject bulletPrefab;
    public int bulletCount = 12;
    public float bulletSpeed = 6f;
    public float spreadAngle = 120f;

    [Header("접촉 데미지")]
    public int contactDamage = 10;
    public float contactDamageCooldown = 1f;

    private Dictionary<Player, float> contactCooldowns = new Dictionary<Player, float>();

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        status = GetComponent<MonsterStatus>();

        var baseStatus = new Status { Hp = 500, Atk = 50 };
        status.InitDungeon(baseStatus, 1);
    }

    protected override void Update()
    {
        base.Update();
        if (target == null) return;

        bombTimer += Time.deltaTime;
        if (bombTimer >= bombCooldown)
        {
            SpawnBomb();
            bombTimer = 0f;
        }

        if (!isJumping)
        {
            patternTimer += Time.deltaTime;
            if (patternTimer >= patternCooldown)
            {
                patternTimer = 0f;
                ChoosePattern();
            }
        }

        UpdateContactCooldowns();
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

        DealJumpDamage();
        isJumping = false;
    }

    private void DealJumpDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.2f, LayerMask.GetMask("Player"));
        foreach (var hit in hits)
        {
            Player p = hit.GetComponent<Player>();
            if (p != null)
            {
                p.TakeDamage(jumpDamage);
                Debug.Log($"점프 착지 공격! {jumpDamage} 데미지");
            }
        }
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryDealContactDamage(other.GetComponent<Player>());
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryDealContactDamage(other.GetComponent<Player>());
    }

    private void TryDealContactDamage(Player player)
    {
        if (player == null) return;

        if (contactCooldowns.ContainsKey(player) && contactCooldowns[player] > 0f)
            return;

        player.TakeDamage(contactDamage);
        Debug.Log($"보스 접촉 피해: {contactDamage}");

        contactCooldowns[player] = contactDamageCooldown;
    }

    private void UpdateContactCooldowns()
    {
        var keys = new List<Player>(contactCooldowns.Keys);
        foreach (var player in keys)
        {
            contactCooldowns[player] -= Time.deltaTime;
            if (contactCooldowns[player] <= 0)
                contactCooldowns.Remove(player);
        }
    }
}
