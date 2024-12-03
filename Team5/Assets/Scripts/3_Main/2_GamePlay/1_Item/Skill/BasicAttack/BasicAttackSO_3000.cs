using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "3000", menuName = "SO/SkillItem/3000_DefaultBasicAttack", order = int.MaxValue)]
public class BasicAttackSO_3000 : BasicAttackSO
{
    public override string id => "3000";

    public override string dataName => "기본기본공격";


    public BasicAttackSO_3000()
    {
        defaultDamage = 30f;
        damageWeight = 1f;
    }
}
