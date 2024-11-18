using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectableItemInfoPanel : GamePlayPanel
{
    [SerializeField] Image img_icon;
    [SerializeField] TextMeshProUGUI text_itemName;
    [SerializeField] TextMeshProUGUI text_itemDesc;

    [SerializeField] Button btn_select;
    [SerializeField] Button btn_reroll;

    ItemDataSO currItemData;


    protected override void Init()
    {
        GameEventManager.Instance.onUpdate_inspectingObject.AddListener( OnUpdate_inspectingObject );
        // GameEventManager.Instance.onUpdate_closestSelectableItem.AddListener( UpdateItemInfo );
        
        // btn_select.onClick.AddListener(()=> GamePlayManager.Instance.OnSelect_SelectableItem(currItem) );
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen()
    {
        
    }

    void OnUpdate_inspectingObject(InteractiveObject interactiveObject)
    {
       
        if( interactiveObject  is  SelectableItem)
        {
            SelectableItem selectableItem = (SelectableItem)interactiveObject;
        
            Open();
            UpdateItemInfo(selectableItem.data);
        }
        else
        {
            Close();
        }
    }



    public void UpdateItemInfo(ItemDataSO itemData)
    {
        currItemData = itemData;

        img_icon.sprite = currItemData.sprite;
        text_itemName.SetText(currItemData.dataName);
        text_itemDesc.SetText(currItemData.description);
    }


    


}
