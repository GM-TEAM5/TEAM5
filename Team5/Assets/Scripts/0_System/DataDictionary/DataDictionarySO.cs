using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/// <summary>
/// 데이터 컨테이너 : list 에 아이템을 추가하면 자동으로 게임에서 사용되는 dic에 추가가 된다.
/// </summary>
public class DataDictionarySO : ScriptableObject
{
    [SerializeField] protected List<GameData> list = new(); 
    
    
    [ReadOnly] public SerializableDictionary<string,GameData> dic = new(); 


    // 유니티 에디터에서 값이 변경될 때마다 호출되는 메서드
    private void OnValidate()
    {
        // 딕셔너리와 리스트를 동기화
        SyncDictionaryWithList();
    }

    // 딕셔너리를 리스트와 동기화하는 메서드
    private void SyncDictionaryWithList()
    {
        // 리스트에서 null인 값이 없을 때, 
        if (list.Any(x=>x==null))
        {
            return;
        }
        
        
        list = list.OrderBy(x=> x.id).ToList();    // id로 오름차순
        
        dic.Clear();

        // 사전에 리스트의 데이터 등록 
        foreach (GameData data in list)
        {
            if (data==null)
            {
                return;
            }
            
            string id = data.id;
            if (!dic.ContainsKey(id))
            {
                dic[id] = data;
            }
        }
    }

    //================================================

    /// <summary>
    /// 중복 없이 dataNum개의 데이터를 list에서 가져온다.
    /// </summary>
    /// <param name="dataNum"></param>
    /// <returns></returns>
    public List<GameData> GetRandomData( int dataNum )
    {
        // 
        List<GameData> ret = new();
        List<int> idxs = new();

        int totalDataNum = list.Count;
        //
        for (int i = 0; i < totalDataNum; i++)
        {
            idxs.Add(i);
        }

        //
        System.Random random = new System.Random();
        
        for (int i = 0; i < dataNum ; i++)
        {
            int idx = random.Next(idxs.Count);  // 랜덤으로 나온 idx
            ret.Add( list[ idxs[idx] ]   );     // 리스트에서 아이템 뽑아냄. 
            idxs.RemoveAt(idx);                 
        }

        //
        return ret;
    }




}
