using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "2000", menuName = "SO/SkillItem/2000_DrawAttack", order = int.MaxValue)]
public class DrawAttackSO_2000 : DrawAttackSO
{
    public override string id => $"2000_{property}";
    public override string dataName =>$"그리기_{property}";

    public DrawAttackSO_2000()
    {
        defaultDamage = 60f;
        damageWeight = 1f;
    }
}
