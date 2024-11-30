using System.Collections;
using System.Collections.Generic;
using BW.Util;
using UnityEngine; 

public class EnemySpawner : MonoBehaviour, IPoolObject
{
    //
    public void OnCreatedInPool()
    {
      
    }

    public void OnGettingFromPool()
    {
      
    }
    
    //
    public void SpawnEnemy(string id, Vector3 initPos, float delay)
    {
        transform.position = initPos+ new Vector3(0,0.01f,0);
        
        StartCoroutine(SpawnSequnce(id, initPos, delay));
    }

    IEnumerator SpawnSequnce(string id, Vector3 initPos, float delay)
    {
        yield return new WaitForSeconds(delay);

        PoolManager.Instance.GetEnemy( id, initPos ); 
        PoolManager.Instance.TakeToPool<EnemySpawner>(this);
    }
}
