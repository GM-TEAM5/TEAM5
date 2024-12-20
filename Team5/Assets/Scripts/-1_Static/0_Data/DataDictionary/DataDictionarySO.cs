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
    
    
    [ReadOnly] 
    public SerializableDictionary<string,GameData> dic = new(); 

    protected System.Random random = new System.Random();


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

    public List<GameData> GetSamePropertySkills(int targetNum, SkillProperty property)
    {
        List<GameData> ret = list.Where(x=>((SkillItemSO)x).property == property).ToList();
        ret = GetRandomUniqueItems(ret, targetNum);

        return ret;
    }


    /// <summary>
    /// 중복 없이 dataNum개의 데이터를 list에서 가져온다.
    /// </summary>
    /// <param name="targetNum"></param>
    /// <returns></returns>
    public List<GameData> GetRandomData( int targetNum, List<GameData> exception=null)
    {
        // 
        List<GameData> ret = GetItemsWithExption(list, exception);
        ret = GetRandomUniqueItems(ret, targetNum);


        //
        return ret;
    }

    /// <summary>
    /// 특정 데이터를 제외한 데이터를 받아온다. 
    /// </summary>
    /// <param name="sourceList"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    public List<GameData> GetItemsWithExption( List<GameData> sourceList, List<GameData> exception)
    {
        if (exception == null || exception.Count == 0)
        {
            return sourceList;
        }

        return sourceList.Except(exception).ToList();
    }

    /// <summary>
    /// 지정된 리스트에서 중복없는 n개의 원소를 얻는다. 
    /// </summary>
    /// <param name="sourceList"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public List<GameData> GetRandomUniqueItems( List<GameData> sourceList, int n)
    {
        if (n > sourceList.Count)
        {
            n = sourceList.Count;
        }
           

        // 랜덤하게 섞기
        return sourceList.OrderBy(x => random.Next()).Take(n).ToList();
    }

    public GameData GetData(string id)
    {
        if (dic.ContainsKey(id))
        {
            return dic[id];
        }

        return null;
    }


}
