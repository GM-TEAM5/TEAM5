using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Wrapper 클래스 정의
[System.Serializable]
public class SoundFileRow
{
    public List<AudioClip> files = new List<AudioClip>();

    public void Init(int rowCount)
    {
        files = files
        .Take(rowCount)
        .Concat(Enumerable.Repeat((AudioClip)null, Mathf.Max(0, rowCount - files.Count)))  // 부족한 부분을 null으로 채움
        .ToList();

    }
}






[CreateAssetMenu(fileName = "RandomCombinationSound", menuName = "SO/Sound/RandomCombination", order = int.MaxValue)]
public class RandomCombinationSoundSO : SoundSO 
{
    public int combinationCount;
    public int randomCount;
    
    public List<SoundFileRow> rows; 

    //===================================================
    void OnValidate()
    { 
        AdjustFilesList();
    }

    // files 리스트의 크기를 combinationCount와 randomCount에 맞게 조정
    private void AdjustFilesList()
    {
        rows = rows
        .Take(combinationCount)
        .Concat(Enumerable.Repeat(new SoundFileRow(), Mathf.Max(0, combinationCount - rows.Count)))  // 부족한 부분을 null으로 채움
        .ToList();

        foreach(SoundFileRow row in rows)
        {
            row.Init(randomCount );
        }
    }


    public override void Play(Transform t)
    {
        int rand = Random.Range(0,randomCount);

        for(int i=0;i<combinationCount;i++)
        {
            AudioClip file = rows[i].files[rand];
            SoundManager.Instance.EnqueueSound(file, type, rank, t.position);
        }
    } 
}



