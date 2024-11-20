using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeSelection : MonoBehaviour
{
    public int idx;
    public ItemDataSO data;

    [SerializeField] Image img_icon;
    [SerializeField] TextMeshProUGUI text_itemName;
    [SerializeField] TextMeshProUGUI text_itemTier;
    [SerializeField] TextMeshProUGUI text_itemDesc;

    [SerializeField] Button btn_select;
    [SerializeField] Button btn_reroll;

    //=============================================================

    public void Init(int idx, ItemDataSO data)
    {
        this.idx = idx;
        this.data = data;

        UpdateItemInfo(data);


        btn_reroll.onClick.AddListener(Reroll); 
    }


    public void UpdateItemInfo(ItemDataSO data)
    {
        img_icon.sprite = data.sprite;
        text_itemName.SetText(data.dataName);
        text_itemTier.SetText($"{data.tier} 등급");
        text_itemDesc.SetText(data.description);
    }

    //==============================================================
    void OnSelect()
    {
        
    }

    void Reroll()
    {
        GamePlayManager.Instance.Reroll(this);
    }

}
