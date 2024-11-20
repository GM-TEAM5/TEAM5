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

    [SerializeField] GameObject vayle;

    //
    bool _initialized;

    //=============================================================
    void Init()
    {
        btn_select.onClick.AddListener(Select);
        btn_reroll.onClick.AddListener(Reroll); 

         _initialized = true;
    }



    public void UpdateItemInfo(int idx, ItemDataSO data)
    {
        if (_initialized == false)
        {
            Init();
        }
        
        //s
        this.idx = idx;
        this.data = data;
        
        img_icon.sprite = data.sprite;
        text_itemName.SetText(data.dataName);
        text_itemTier.SetText($"{data.tier} 등급");
        text_itemDesc.SetText(data.description);

        btn_select.gameObject.SetActive(true);  
        btn_reroll.gameObject.SetActive(Player.Instance.status.rerollCount > 0);

        vayle.SetActive(false);
    }

    //==============================================================
    void Select()
    {
        GamePlayManager.Instance.Select_SelectableItem(data);
        vayle.SetActive(true);
    }

    void Reroll()
    {
        GamePlayManager.Instance.Reroll(this);
    }

}
