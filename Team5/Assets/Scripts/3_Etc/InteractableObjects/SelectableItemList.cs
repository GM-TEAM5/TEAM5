using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System.Linq;
using Unity.Collections;
using System;

public class SelectableItemList : InteractableObject
{
    [SerializeField] TextMeshPro text;
    [SerializeField] Transform t_itemList;
    [SerializeField] SelectableItem[] t_items;
    SelectableItem closestItem;

    
    

    //========================================================
    protected override void OnEnter(bool isOn)
    {
        text.gameObject.SetActive(isOn);
        // Debug.Log($" 엔떠{ isOn} ");

        GameEventManager.Instance.onCloseTo_selectableItemList.Invoke(isOn);
    }

    protected override void OnInteract()
    {
        GamePlayManager.Instance.Select_SelectableItem(closestItem);
    }


    //========================================================

    protected override void Update()
    {
        base.Update();

        //
        if (isPlayerInRange)
        {
            
            SelectableItem newClosestItem = GetClosestItem();
            
            if (newClosestItem  ==null && closestItem == null)
            {
                OnEnter(false);
            }
            else if(closestItem != newClosestItem)
            {
                closestItem = newClosestItem;
                GameEventManager.Instance.onUpdate_closestSelectableItem.Invoke(closestItem);
            }
        }
    }


    public void Init()
    {
        t_items = new SelectableItem[t_itemList.childCount];
        for(int i=0;i<t_items.Length;i++)
        {
            t_items[i] = t_itemList.GetChild(i).GetComponent<SelectableItem>();
            t_items[i].Init(i); 
        }

        OnWaveStart();
    }



    //=======================================================

    SelectableItem GetClosestItem()
    {
        SelectableItem ret = null;

        Vector3 playerPos = Player.Instance.t_player.position;
        ret =  t_items
            .OrderBy(t => Vector3.Distance(t.transform.position, playerPos))
            .FirstOrDefault();

        return ret;
    }


    public void OnWaveStart()
    {
        gameObject.SetActive(false);
        Deactivate();
    }


    public void OnWaveClear()
    {
        gameObject.SetActive(true);
        Activate();
    }
    
}
