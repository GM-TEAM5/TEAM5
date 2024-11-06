using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(BillboardSprite))]
public class SelectableItem : MonoBehaviour
{
    SpriteRenderer _sr; 
    SpriteRenderer sr
    {
        get
        {
            if( _sr ==null)
            {
                _sr = GetComponent<SpriteRenderer>();
            }
            return _sr;
        }
    }


    public ItemDataSO data;


    public void Init(ItemDataSO itemData)
    {
        data = itemData;
        sr.sprite = itemData.sprite;

        // debugText.SetText(i.ToString());
    }
}
