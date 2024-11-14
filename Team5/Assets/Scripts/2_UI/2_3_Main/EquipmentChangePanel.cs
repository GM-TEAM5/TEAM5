using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class EquipmentChangePanel : GamePlayPanel
{
    [SerializeField] PlayerEquipmentInfoUI playerEquipmentsUI;
    // [SerializeField] List<Toggle> toggles;
    [SerializeField] ToggleGroup toggleGroup;
    
    [SerializeField] Button btn_ok;
    [SerializeField] Button btn_cancel;

    //
    [SerializeField] EquipmentItemSO selectedEquipment;
    [SerializeField] PlayerItemUI disposalEquipment;


    //===========================================================================================

    protected override void Init()
    {
        //
        // btn_ok = GetComponent<Button>();
        btn_ok.onClick.AddListener(OnOk);
        //
        // btn_cancel = GetComponent<Button>();
        btn_cancel.onClick.AddListener( Close ); 

        // playerEquipmentsUI.Init();
        //
        // foreach (var playeritem in playerEquipmentsUI.playerItems)
        // {
            // Toggle toggle = playeritem.GetComponent<Toggle>();
            // toggle.group = toggleGroup;

            // toggle.onValueChanged.AddListener((isSelected) => OnToggleValueChanged(toggle, isSelected));
        // }

        
    }

    protected override void OnOpen()
    {
        playerEquipmentsUI.UpdateItems();
        //
        foreach (var playeritem in playerEquipmentsUI.playerItems)
        {
            Toggle toggle = playeritem.GetComponent<Toggle>();
            toggle.enabled = playeritem.data != null;

            if( toggle.group ==null)
            {
                toggle.group = toggleGroup;
            }
        }
        toggleGroup.SetAllTogglesOff(); 

        disposalEquipment = null;
        btn_ok.interactable = false;
    }


    protected override void OnClose()
    {
        
    }

    //===================================
    void Update()
    {
        Toggle toggle = toggleGroup.GetFirstActiveToggle();
        if (toggle != null)
        {
            PlayerItemUI itemUI = toggle.GetComponent<PlayerItemUI>();
            disposalEquipment = itemUI;
            
            btn_ok.interactable = disposalEquipment.data != null;
        }
        else
        {
            btn_ok.interactable = false;
        }
    }


    //================================
    
    public void InitSelectedEquipment(EquipmentItemSO selectedEquipment)
    {
        this.selectedEquipment = selectedEquipment;

    }


    // void OnToggleValueChanged(Toggle changedToggle, bool isSelected)
    // {
    //     if (isSelected)
    //     {
    //         selectedEquipment = (EquipmentItemSO)changedToggle.GetComponent<PlayerItemUI>().data;
    //         btn_ok.interactable = selectedEquipment != null;
    //     }
    // }
    

    void OnOk()
    {
        // 장착.
        int idx = disposalEquipment.idx;
        Player.Instance.Equip(idx, selectedEquipment);
        GamePlayManager.Instance.FinishSelection();

        //
        Close();
    }


}
