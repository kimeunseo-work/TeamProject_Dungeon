//using UnityEngine;
//public abstract class PassiveSkill : MonoBehaviour
//{
//    [Header("Passive Info")]
//    public string skillName = "New Passive";
//    public int level = 1; // 패시브 레벨

//    // 패시브 효과를 대상 스킬에 적용
//    public abstract void ApplyEffect(Skill targetSkill);

//    // 선택지에서 레벨업 시 호출
//    public void LevelUp(Skill targetSkill)
//    {
//        level++;
//        ApplyEffect(targetSkill);
//        Debug.Log($"{skillName} 레벨업! 현재 레벨: {level}");
//    }
//}
