using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite icon;
    public GameObject skillPrefab;
    public SkillType type;
    public bool canStack = false;
}

public enum SkillType
{
    Active,
    Pasive,
}