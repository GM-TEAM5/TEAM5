// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class EnemySkill 
// {    
//     public EnemyAbilitySO skillData;
    
//     public int useCount; 
//     public float lastUseTime;   // 스킬 마지막 사용시간 

//     public float cooltimeRemain => lastUseTime + skillData.cooltime - Time.time;
//     public bool isCooltimeOk => cooltimeRemain <= 0;

//     Coroutine skillRoutine;

//     public EnemySkill(EnemyAbilitySO skillData)
//     {
//         this.skillData = skillData;
//         lastUseTime = -skillData.cooltime + 1 ; // 획득 후 1초 후에 사용되도록.
//     }


//     public void Use(Enemy enemy, Vector3 targetPos)
//     {
//         useCount ++;
        
//         //        
//         skillRoutine = enemy.StartCoroutine( UseSkillRoutine(enemy, targetPos));
//     }

//     IEnumerator UseSkillRoutine( Enemy enemy, Vector3 targetPos )
//     {
//         // 스킬 상태 진입.
//         yield return new WaitForSeconds( skillData.delay_beforeCast );
//         if (enemy.isAlive == false)
//         {
//             yield break;
//         }
//         skillData.Use( enemy, targetPos );
//         yield return new WaitForSeconds( skillData.delay_afterCast );
//         if (enemy.isAlive == false)
//         {
//             yield break;
//         }
//         // 스킬 상태 해제 
//         // enemy.OnFinish_Skill();
//         lastUseTime = Time.time; //시간기록
//     }

//     public void Interrupt(Enemy enemy)
//     {
//         if (skillData.interruptionDefense==false)
//         {
//             enemy.StopCoroutine( skillRoutine );
//         }
//     }
// }
