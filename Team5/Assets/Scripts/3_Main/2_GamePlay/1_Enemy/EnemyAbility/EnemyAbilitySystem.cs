using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAbilitySystem : MonoBehaviour
{
    Enemy enemy;
    
    public List<EnemyAbility> abilities = new();
    public EnemyAbility usingAbility;
    public bool isCasting;

    Coroutine abilityRoutine;


    public float currAbilityRange
    {
        get
        {
            float ret = 1f;
            if( usingAbility != null)
            {
                ret = usingAbility.data.range;
            }
            return ret;
        }
    }
    
    //==================================================

    public void Init(Enemy enemy)
    {    
        this.enemy = enemy;
        usingAbility = null;
        isCasting = false;

        //
        abilities.Clear();
        foreach (var abilityData in enemy.data.abilities)
        {
            EnemyAbility ability = new(abilityData);
            abilities.Add(ability);
        }
    }

    public void SetCurrAbility()
    {
        EnemyAbility ability = abilities
        .Where(a => a.CanUse(enemy)) 
        .OrderByDescending(a => a.data.priority)
        .ThenBy(a => a.useCount)
        .FirstOrDefault(); 

        if (ability ==null)
        {
            // 사용할 수 있는 능력들중 우선순위 가장 높은 거
            ability = abilities
                .Where(a => a.CanActiavte(enemy)) 
                .OrderByDescending(a => a.data.priority)
                .FirstOrDefault(); 
        }


        usingAbility = ability;
    }


    public void TryUse()
    {
        if (usingAbility == null || usingAbility.CanUseImediatly(enemy)== false)
        {
            return;
        }

        abilityRoutine = StartCoroutine(UseAbility( enemy ));
    }

    
    IEnumerator UseAbility( Enemy enemy )
    {
        // 스킬 상태 진입.
        isCasting = true;
        
        Vector3 castingPos = enemy.t_target.position;
        usingAbility.data.StartCast(enemy);
        yield return new WaitForSeconds( usingAbility.data.castingTime );
        usingAbility.ApplyAbility( castingPos, enemy );
        yield return new WaitForSeconds( usingAbility.data.delay_afterCast );
        
        // 스킬 상태 해제 
        usingAbility = null;
        isCasting = false;
    }


    // bool CanUse(EnemySkill skill)
    // {
    //     bool targetInRange = targetDistSqr <= range * range * 1.1f;

    //     return targetInRange && skill.isCooltimeOk;
    // }

    public void Interrupt(Enemy enemy)
    {
        if(usingAbility == null || usingAbility.data.uninterruptible)
        {
            return;
        }
        
        if (abilityRoutine != null)
        {
            StopCoroutine( abilityRoutine );
        }
        usingAbility = null;
        isCasting = false;
    }

    public void OnDie()
    {
        if (abilityRoutine != null)
        {
            StopCoroutine( abilityRoutine );
        }
        usingAbility = null;
        isCasting = false;
    }

}
