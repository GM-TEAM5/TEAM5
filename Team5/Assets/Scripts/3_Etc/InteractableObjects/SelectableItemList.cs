using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System.Linq;
using Unity.Collections;
using System;

public class SelectableItemList : MonoBehaviour
{
    [SerializeField] TextMeshPro text;
    [SerializeField] Transform t_itemList;
    [SerializeField] SelectableItem[] items;

    
    

    public void Init()
    {
        items = new SelectableItem[t_itemList.childCount];
        for(int i=0;i<items.Length;i++)
        {
            items[i] = t_itemList.GetChild(i).GetComponent<SelectableItem>();
        }

        //
        Deactivate();
    }



    //=======================================================
    public void Deactivate()
    {
        gameObject.SetActive(false);
        ActiavteItems(false);
    }


    public void OnWaveStart()
    {
        Deactivate();
    }


    public void OnWaveClear()
    {
        FillItemData();
        ActiavteItems(true);

        gameObject.SetActive(true);
        
    }

    //==============================

    void FillItemData()
    {
        List<GameData> randomItemData = ResourceManager.Instance.itemData.GetRandomData(4);

        for(int i=0;i<4;i++)
        {
            SelectableItem si = items[i];
            ItemDataSO itemData = (ItemDataSO)randomItemData[i];

            si.Init(i,itemData);
        }
    }

    public void Reroll(SelectableItem selectableItem)
    {
        int idx = selectableItem.idx;

        List<GameData> exception = items.Select(x=> (GameData)x.data).ToList();
        List<GameData> randomItemData = ResourceManager.Instance.itemData.GetRandomData(1, exception );
        if (randomItemData.Count>0)
        {
            items[idx].Init(idx, (ItemDataSO)randomItemData[0] );
        }

        //
        items[idx].OnInspect(true);
    }



    void ActiavteItems(bool isOn)
    {
        for(int i=0;i<items.Length;i++)
        {
            items[i].gameObject.SetActive(isOn);
        }

        //
    
    }
    
}
