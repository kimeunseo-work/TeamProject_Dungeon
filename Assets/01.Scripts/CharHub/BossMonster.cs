using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : Character
{
    [Header("보스 스텟")]
    public int maxHp = 500;
    private int currentHp;

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

    private Transform player;
    private Rigidbody2D rb;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        currentHp = maxHp;
    }

    protected override void Update()
    {
        base.Update();

        if (player == null) return;

        // 폭탄 패턴은 별도 타이머
        bombTimer += Time.deltaTime;
        if (bombTimer >= bombCooldown)
        {
            SpawnBomb();
            bombTimer = 0f;
        }

        // 돌진/점프 등 공격 패턴 관리
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

    // 돌진 공격
    private IEnumerator DashAttack()
    {
        isDashing = true;
        float elapsed = 0f;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * dashSpeed;

        while (elapsed < dashDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        isDashing = false;
    }

    // 점프 공격
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

        // 내려찍기
        elapsed = 0f;
        Vector3 targetPos = new Vector3(player.position.x, startPos.y, startPos.z);
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
        if (bombPrefab == null || player == null) return;

        Vector3 spawnPos = player.position + new Vector3(
            Random.Range(-bombSpawnRange.x, bombSpawnRange.x),
            Random.Range(-bombSpawnRange.y, bombSpawnRange.y),
            0f
        );

        GameObject bomb = Instantiate(bombPrefab, spawnPos, Quaternion.identity);
        Bomb bombScript = bomb.GetComponent<Bomb>();
        if (bombScript != null)
            bombScript.explosionRadius = bombExplosionRadius;
    }

    protected override void Attack() { }

    public override void TakeDamage(int amount)
    {
        currentHp -= amount;
        if (currentHp <= 0)
        {
            currentHp = 0;
            Status_OnDead();
        }
    }

    protected override void Status_OnDead()
    {
        Debug.Log("보스 사망!");
        Destroy(gameObject);
    }
}