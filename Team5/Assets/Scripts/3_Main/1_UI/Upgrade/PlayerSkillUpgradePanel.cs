using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;

public class PlayerSkillUpgradePanel : GamePlayPanel
{
    

    // Selections
    [Header("Selections")]
    [SerializeField] GameObject prefab_selection;
    [SerializeField] Transform t_selections;
    [SerializeField] List<UpgradeSelection> selections;
    //
    [Header("Selection")]
    [SerializeField] ItemDataSO selectedSkill;
    [SerializeField] Button btn_select; 

    [Header("Etc")]
    [SerializeField] Button btn_close; 
    // [SerializeField] TextMeshProUGUI text_rerollCount; 

    //====================================================================
    
    protected override void Init()
    {
        //
        selections = new();
        for(int i=0;i<t_selections.childCount;i++)
        {
            selections.Add(t_selections.GetChild(i).GetComponent<UpgradeSelection>());
        }

        //
        btn_close.onClick.AddListener( GamePlayManager.Instance.CloseSkillUpgradePanel );
    }

    protected override void OnOpen()
    {   
        SetRerollcount();
        SyncSlectionCount();
        FillSelectionData();
    }

    protected override void OnClose()
    {
        
    }
    
    //=====================================================================
    
    
    void SyncSlectionCount()
    {
        int targetSelectionCount = Player.Instance.status.selectionCount;
        int currSelectionCount = selections.Count;

        int diff = targetSelectionCount - currSelectionCount;

        // 현재 선택지 개수가 부족하면 프리팹 생성
        if(diff>0 )
        {
            for(int i=0;i<diff;i++)
            {
                selections.Add( Instantiate(prefab_selection,t_selections).GetComponent<UpgradeSelection>() );
            }
        }
        // 현재 선택지 개수가 초과했으면 몇개 파괴
        else if( diff <0)
        {
            for(int i=0; i> diff;i-- )
            {
                int idx = currSelectionCount+i-1; 
                Destroy(selections[idx].gameObject);
                selections.RemoveAt(idx);
            }
        }

        //
        // Debug.Log($" 선택지 개수 동기화 {targetSelectionCount} , { currSelectionCount} => { selections.Count}");
    }


    
    void FillSelectionData()
    {
        int targetSelectionCount  = selections.Count;
        
        List<GameData> randomItemData = ResourceManager.Instance.itemDic.GetRandomData( targetSelectionCount );

        for(int i=0;i<targetSelectionCount;i++)
        {
            UpgradeSelection us = selections[i];
            ItemDataSO itemData = (ItemDataSO)randomItemData[i];

            us.UpdateItemInfo(i,itemData);
        }
    }

    
    public void Reroll( UpgradeSelection selection )
    {
        int idx = selection.idx;

        List<GameData> exception = selections.Select(x=> (GameData)x.data).ToList();
        List<GameData> randomItemData = ResourceManager.Instance.itemDic.GetRandomData(1, exception );
        if (randomItemData.Count>0)
        {
            selections[idx].UpdateItemInfo(idx,(ItemDataSO)randomItemData[0] );
        }

        //
        SetRerollcount();
    }


    void SetRerollcount()
    {
        // text_rerollCount.SetText($"재입고 가능 횟수 : {Player.Instance.status.rerollCount}" );
    }


}
