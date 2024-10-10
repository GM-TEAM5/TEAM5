using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(fileName = "Skill_01_Pistol", menuName = "SO/PlayerSkill/01")]
public class Skill_01_Pistol :PlayerSkillSO  
{
    

    public override Vector3 FindTargetPos()
    {
        Vector3 playerPos = Player.Instance.t_player.position;
        Vector3 ret = playerPos;

        Collider[] hits = Physics.OverlapSphere(playerPos, 10f, Util.GameConstants.enemyLayer);         

        if (hits.Length>0)
        {
            Collider closestHit = hits.OrderBy(hit => (hit.transform.position-playerPos).sqrMagnitude  ).FirstOrDefault();
            ret  = closestHit.transform.position; 
        }

        return ret;
    }
    
    public override void Use(Vector3 targetPos)
    {
        Debug.Log("[스킬] 빵야");
    }
}
