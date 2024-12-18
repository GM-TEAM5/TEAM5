using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(fileName = "WaveDictionary", menuName = "SO/Dictionary/Wave", order = int.MaxValue)]
public class WaveDictionarySO : ScriptableObject
{
    [SerializeField] protected List<WaveDataSO> list_normal;
    [SerializeField] protected List<WaveDataSO> list_middleBoss;
    [SerializeField] protected List<WaveDataSO> list_ChapterBoss;
    

    [ReadOnly] public SerializableDictionary<int,List<WaveDataSO>> normalWaves = new();           // 일반 몬스터 웨이브 
    [ReadOnly] public SerializableDictionary<int,List<WaveDataSO>> middleBossWaves= new();  //  중간보스 웨이브
    [ReadOnly] public SerializableDictionary<int,List<WaveDataSO>> chapterBossWaves = new();    //  챕터보스 웨이브

    //==============================================================================

    public WaveDataSO GetNormalWave(int rank)
    {
        return GetWave(normalWaves, rank);
    }

    public WaveDataSO GetMiddleBossWave(int chapter)
    {
        return GetWave(middleBossWaves, chapter);
    }

    public WaveDataSO GetChapterBossWave(int chapter)
    {
        return GetWave(chapterBossWaves, chapter);
    }

    WaveDataSO GetWave(SerializableDictionary<int,List<WaveDataSO>> dic, int value)
    {
        // 키 값이 이상한 경우 예외처리
        if (dic.ContainsKey(value)==false)
        {
            value = dic.Keys.Max();    
        }
        
        //
        List<WaveDataSO> waveList  = dic[value];
        int randIdx  = BW.Math.GetRandom(0, waveList.Count);

        WaveDataSO ret = waveList[randIdx];

        return ret;
    }












    //===========================================================================================


    // 유니티 에디터에서 값이 변경될 때마다 호출되는 메서드
    private void OnValidate()
    {
        // 딕셔너리와 리스트를 동기화
        SyncDictionaryWithList(list_normal, normalWaves);
        SyncDictionaryWithList(list_middleBoss, middleBossWaves);
        SyncDictionaryWithList(list_ChapterBoss, chapterBossWaves);
    }

    // 딕셔너리를 리스트와 동기화하는 메서드
    private void SyncDictionaryWithList(List<WaveDataSO> list, SerializableDictionary<int,List<WaveDataSO>> dic)
    {
        // 리스트에서 null인 값이 없을 때, 
        if (list.Any(x=>x==null))
        {
            return;
        }
        
        
        list = list.OrderBy(x=> x.rank).ToList();    // id로 오름차순
        
        dic.Clear();

        // 사전에 리스트의 데이터 등록 
        foreach (WaveDataSO wave in list)
        {
            if (wave==null)
            {
                continue;
            }
            
            int rank = wave.rank;
            if (dic.ContainsKey(rank) == false)
            {    
                dic[rank] = new();
            }

            dic[rank].Add(wave);
        }
    }

    //================================================

    

}
