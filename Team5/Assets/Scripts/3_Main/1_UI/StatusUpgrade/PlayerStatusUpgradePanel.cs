using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUpgradePanel : GamePlayPanel
{
    [Header("Progress")]
    [SerializeField] GameObject prefab_progressUI;
    [SerializeField] Transform t_progressUIs;

    List<PlayerStatusUpgradeProgressUI> progressUIs;

    [Header("Info")]
    [SerializeField] TextMeshProUGUI text_effect;
    [SerializeField] TextMeshProUGUI text_cost;
    


    [Header("Btns")]
    [SerializeField] Button btn_adjust;
    [SerializeField] Button btn_close;
    

    [Header("Etc")]
    [SerializeField] Color color_canAdjust = Color.black;
    [SerializeField] Color color_unvalid = new Color(0.8f,0,0);

    [SerializeField] PlayerStatusUpgradeProgressUI selectedUI;

    //============================================================
    protected override void Init()
    {
        for(int i=0;i<t_progressUIs.childCount;i++)
        {
            Destroy(t_progressUIs.GetChild(i).gameObject);
        }

        progressUIs = new();
        PlayerStatusUpgradeProgress data = Player.Instance.statusUpgradeProgress;
        ToggleGroup toggleGroup = t_progressUIs.GetComponent<ToggleGroup>();
        foreach(var kv in data.list)
        {
            PlayerStatusUpgradeProgressData field = kv.Value;

            PlayerStatusUpgradeProgressUI progressUI = Instantiate(prefab_progressUI, t_progressUIs).GetComponent<PlayerStatusUpgradeProgressUI>();
            progressUI.Init(field, toggleGroup, this);

            progressUIs.Add(progressUI);
        }

        //
        btn_close.onClick.AddListener( GamePlayManager.Instance.ClosePlayerStatusUpgradePanel );
        btn_adjust.onClick.AddListener(TryAdjust);
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen()
    {
        foreach(var progressUI in progressUIs)
        {
            progressUI.OnOpen();
        }
    }

    //======================================================
    public void DisplayData(PlayerStatusUpgradeProgressUI progressUI)
    {
        selectedUI = progressUI;
        PlayerStatusUpgradeProgressData data = selectedUI.data;
        //
        text_effect.SetText($"{data.effectInfo}"); 
        
        //
        int cost = data.cost;
        int gainedPoints=  Player.Instance.status.statusUpgradePoint;


        Color stateColor = data.canUpgrade? color_canAdjust:color_unvalid;
        string hex = stateColor.ToHexString();
        text_cost.SetText($"<color=#{hex}>{cost}</color>/{gainedPoints}");


        //
        btn_adjust.interactable = data.canUpgrade;

    }


    public void TryAdjust()
    {
        if(selectedUI!=null)
        {
            PlayerStatusUpgradeProgressData data = selectedUI.data;
            if(data.canUpgrade)
            {
                data.Upgrade();
                DisplayData(selectedUI);
                
                
                selectedUI.OnOpen();
            }
            else
            {
                Debug.Log("업그레이드 불가 : 비정상 적인 상황");
            }
        }
    }


}
