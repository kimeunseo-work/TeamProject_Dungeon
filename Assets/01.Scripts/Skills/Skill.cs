using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [Header("Skill Info")]
    public string skillName = "New Skill"; //이름 나중에 생각
    public float cooldown = 2f; //쿨타임
    protected bool isRunning = false; //스킬 사용중인지

    private Player player;
    private bool canAttack = true;
    private float timer = 0f;
    private bool isSkillReady = true;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        player.OnCanAttackChanged += Player_OnCanAttackChanged;
    }

    private void OnDisable()
    {
        player.OnCanAttackChanged -= Player_OnCanAttackChanged;
    }

    protected virtual void Start()
    {
        Debug.Log($"[Skill] canAttack = {canAttack}");
    }

    private void FixedUpdate()
    {
        AutoCastRoutine();
    }

    private void Player_OnCanAttackChanged(bool canAttack)
    {
        this.canAttack = canAttack;
        
    }

    private void AutoCastRoutine()
    {
        // 공격 상태는 아닌데 쿨은 찼을 때
        if (!canAttack && timer >= cooldown)
        {
            if (isSkillReady) return;

            Debug.Log($"[Skill] Skill Already");
            isSkillReady = true;
        }
        // 쿨 돌려야 할 때
        else if (!canAttack && timer < cooldown || canAttack && timer < cooldown)
        {
            timer += Time.deltaTime;
            return;
        }
        // 공격 상태인데 쿨도 찼을 때
        else if (canAttack && timer >= cooldown)
        {
            Debug.Log($"[Skill] Skill Ready & Cast");

            Activate();
            timer = 0f;
            isSkillReady = false;
        }
    }

    //public IEnumerator AutoCastRoutine()
    //{
    //    isRunning = true;

    //    while (isRunning)
    //    {
    //        Activate();
    //        yield return new WaitForSeconds(cooldown);
    //    }
    //}

    protected abstract void Activate();
}
