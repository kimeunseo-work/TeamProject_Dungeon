using UnityEngine;

public class ArrowCountPassive : PassiveSkill
{
    public int arrowsPerLevel = 1;

    public override void ApplyEffect(Skill targetSkill)
    {
        AutoArrowSkill arrowSkill = targetSkill as AutoArrowSkill;
        if (arrowSkill != null)
        {
            arrowSkill.arrowCount = level * arrowsPerLevel;
            Debug.Log($"{skillName} 적용: 화살 개수 = {arrowSkill.arrowCount}");
        }
    }
}
