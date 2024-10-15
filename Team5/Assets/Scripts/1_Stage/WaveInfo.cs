using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 적들의 등장에 관한 데이터  - StageDataSO 에서 사용됨. 
/// </summary>
[System.Serializable]
public class WaveInfo
{
    public List<string> enemyIds= new();
    public int n;           // 해당 웨이브 생성 수 
    public WaveFormSO waveForm;
    public float startTime; // 해당 웨이브 시작시간
    public float endTime;   // 해당 웨이브가 더이상 등장하지 않는 시간
    
    
    [Min(1f)] public float frequency; //  n 초마다 웨이브가 반복할건지, 

    
    
    
    // 이건 나중에 시간별로 해당 웨이브의 생성 빈도를 조정하기 위함
    public AnimationCurve testCurve = new AnimationCurve(       
                                                        new Keyframe(0.0f, 0.0f),  // 첫 번째 키프레임
                                                        new Keyframe(10.0f, 1.0f)   // 두 번째 키프레임);
                                                    );




    public IEnumerator WaveRoutine()
    {
        yield return new WaitForSeconds(startTime);        
        //
        while(true)
        {
            for(int i=0;i<n;i++)
            {
                SpawnWave();
            }

            yield return new WaitForSeconds(frequency);
        }
    }





    //
    public void SpawnWave()
    {
        Vector3 randomPos = new Vector3( Random.Range(-10f,10f), 0, Random.Range(-10f,10f) );

        SpawnWave(randomPos);
    }

    public void SpawnWave(Vector3 spawnPoint)
    {
        waveForm.Spawn( enemyIds ,spawnPoint);
    }
}

