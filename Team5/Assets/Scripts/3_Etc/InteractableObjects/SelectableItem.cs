using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SelectableItem : InteractiveObject
{
    SpriteRenderer _sr; 
    SpriteRenderer sr
    {
        get
        {
            if( _sr ==null)
            {
                _sr = GetComponentInChildren<SpriteRenderer>();
            }
            return _sr;
        }
    }


    public ItemDataSO data;


    [SerializeField] TextMeshPro text_select;


    //======================================================================


    public void Init(ItemDataSO itemData)
    {
        data = itemData;
        sr.sprite = itemData.sprite;

        // debugText.SetText(i.ToString());
    }

    protected override void OnInspect_Custom(bool isOn)
    {
        text_select.gameObject.SetActive(isOn);
        
        GameEventManager.Instance.onCloseTo_selectableItemList.Invoke(isOn);
    }

    protected override void OnInteract_Custom()
    {
        GamePlayManager.Instance.Select_SelectableItem(data);
    }
}
