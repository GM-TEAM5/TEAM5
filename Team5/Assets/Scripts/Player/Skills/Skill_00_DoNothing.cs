using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_00_DoNothing", menuName = "SO/PlayerSkill/00")]
public class Skill_00_DoNothing :PlayerSkillSO  
{
    public override Vector3 FindTargetPos()
    {
        return Player.Instance.t_player.position;
    }

    public override void Use(Vector3 targetPos)
    {
        Debug.Log("[스킬] 아무것도 안하기!");
    }
}
