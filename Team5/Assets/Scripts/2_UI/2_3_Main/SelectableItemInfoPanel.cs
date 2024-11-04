using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectableItemInfoPanel : GamePlayPanel
{
    [SerializeField] Image img_icon;
    [SerializeField] TextMeshProUGUI text_itemName;
    [SerializeField] TextMeshProUGUI text_itemDesc;

    [SerializeField] Button btn_select;

    SelectableItem currItem;


    protected override void Init()
    {
        GameEventManager.Instance.onCloseTo_selectableItemList.AddListener( OnCloseTo_SelectableItemList );
        GameEventManager.Instance.onUpdate_closestSelectableItem.AddListener( UpdateItemInfo );
        
        // btn_select.onClick.AddListener(()=> GamePlayManager.Instance.OnSelect_SelectableItem(currItem) );
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen()
    {
        
    }

    void OnCloseTo_SelectableItemList(bool isOn)
    {
        if (isOn)
        {
            Open();
        }
        else
        {
            Close();
        }
    }



    public void UpdateItemInfo(SelectableItem selectableItem)
    {
        currItem = selectableItem;

        text_itemName.SetText(currItem.debugText.text);
        text_itemDesc.SetText(currItem.debugText.text);
    }


    


}
