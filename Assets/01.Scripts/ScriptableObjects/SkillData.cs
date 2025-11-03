using UnityEngine; 

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/SkillData")]
public class SkillData : ScriptableObject
{
    public string SkillName;
    public string SkillDescription;
    public Sprite Icon;
    public GameObject SkillPrefab;
    public SkillType Type;
    public bool CanStack = false;

    public ActiveSkillType ActiveSkillType;
    public float CoolDown;
    public float Timer;
    public float ArrowSpeed;
    public int ArrowCount;
    public int ExtraPierce;
    public float SpreadAngle;
    public float ShotInterval;
    public bool CanPierce = false;
}

public enum SkillType
{
    Active,
    Passive,
}

public enum ActiveSkillType
{
    None,
    ArrowCount,
    ExtraPierce,
    CoolDown
}