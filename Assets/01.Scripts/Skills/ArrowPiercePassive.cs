using UnityEngine;
public class ArrowPiercePassive : PassiveSkill
{
    public int piercePerLevel = 1;

    public override void ApplyEffect(Skill targetSkill)
    {
        AutoArrowSkill arrowSkill = targetSkill as AutoArrowSkill;
        if (arrowSkill != null)
        {
            arrowSkill.extraPierce = level * piercePerLevel;
            Debug.Log($"{skillName} 적용: 관통 수 = {arrowSkill.extraPierce}");
        }
    }
}
