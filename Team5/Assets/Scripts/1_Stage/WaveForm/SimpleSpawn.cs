using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleSpawn", menuName = "SO_Enum/WaveForm/SimpleSpawn", order = int.MaxValue)] 
public class SimpleSpawn : WaveFormSO
{
    public override void Spawn(List<string> enemyIds, Vector3 spawnPoint)
    {
        //
        for(int i=0;i<enemyIds.Count;i++)
        {
            string id = enemyIds[i];
            
            // 해당 에너미 데이터 가져오고
            // 에너미 생성해서//  Instantiate(  , spawnPoint, Quaternion.identity);
            // 세팅하기. 

            Debug.Log($"적 생성 {id} at {spawnPoint}");
        }
        
    }
}
