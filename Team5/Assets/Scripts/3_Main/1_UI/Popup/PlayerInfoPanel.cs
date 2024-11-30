using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : GamePlayPanel 
{
    [SerializeField] PlayerEquipmentInfoUI playerEquipmentsUI;
    [SerializeField] PlayerSkillInfoUI playerSkillsUI;
    

    [SerializeField] Button btn_setting;
    [SerializeField] Button btn_resume;
    [SerializeField] Button btn_lobby;

    
    protected override void Init()
    {
        btn_setting.onClick.AddListener(()=>{} );
        btn_resume.onClick.AddListener(()=>{ GamePlayManager.Instance.ClosePlayerInfoPanel(); });
        btn_lobby.onClick.AddListener(()=>{  SceneLoadManager.Instance.Load_Lobby(); });

    }

    protected override void OnOpen()
    {
        playerEquipmentsUI.UpdateItems();   
        playerSkillsUI.UpdateItems();
    }

    protected override void OnClose()
    {
        
    }

    



}
