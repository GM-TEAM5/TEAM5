using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/// <summary>
/// 데이터 컨테이너 : list 에 아이템을 추가하면 자동으로 게임에서 사용되는 dic에 추가가 된다.
/// </summary>
public class DataDictionarySO : ScriptableObject
{
    [SerializeField] List<GameData> list = new(); 
    
    
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



}
