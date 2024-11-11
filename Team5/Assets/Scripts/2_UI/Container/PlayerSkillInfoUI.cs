using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillInfoUI : MonoBehaviour
{
    [SerializeField] GameObject prefab_itemUI;
    [SerializeField] GridLayoutGroup gridLayout;
    [SerializeField] List<PlayerItemUI> playerItems;

    bool initialized;
    
    
    public void Init()
    {
        for(int i=0;i<gridLayout.transform.childCount;i++)
        {
            playerItems.Add( gridLayout.transform.GetChild(i).GetComponent<PlayerItemUI>() );
        }
        
        initialized = true;
    }

    public void UpdateItems()
    {
        if( initialized == false)
        {
            Init();            
        }
        
        // 일단 grid의 아이템 개수를 목표 개수로 맞춤
        SyncTableItemCount();
        
        // 현재 상태 만들기
        FillTableItemData();

    }



    /// <summary>
    /// 테이블 아이템 개수를 목표 개수로 맞춘다. 
    /// </summary>
    void SyncTableItemCount()
    {
        int currItemCount = playerItems.Count;
        int targetItemCount = GameManager.Instance.playerData.skills.Count;

        int diff = targetItemCount - currItemCount;

        // grid cell 개수와 list 원소의 개수가 동일하다고 가정.
        if (diff > 0)   // 부족한 경우 채움.
        {
            for(int i=0;i< diff; i++)
            {
                playerItems.Add(  Instantiate(prefab_itemUI,gridLayout.transform).GetComponent<PlayerItemUI>() );
            }
            
        }   
        else if (diff < 0)  // 초과하는 경우 리스트에서 제거
        {
            for(int i=0;i<-diff;i++)
            {
                PlayerItemUI item = playerItems[currItemCount -1 - i];
                playerItems.RemoveAt( currItemCount -1 - i );
                Destroy( item.gameObject );   
            }
        }     

    }

    /// <summary>
    /// 그리드 아이템의 데이터를 채운다.
    /// </summary>
    void FillTableItemData()
    {
        List<SkillItemSO> skillsData =  GameManager.Instance.playerData.skills;

        for(int i=0;i<skillsData.Count;i++)
        {
            playerItems[i].Init( skillsData[i] );
        }

    }
}
