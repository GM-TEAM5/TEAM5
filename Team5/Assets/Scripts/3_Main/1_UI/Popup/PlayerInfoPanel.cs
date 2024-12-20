using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PlayerInfoPanel : GamePlayPanel 
{
    // [SerializeField] PlayerEquipmentInfoUI playerEquipmentsUI;
    [SerializeField] PlayerSkillInfoUI playerSkillsUI;
    

    [SerializeField] Button btn_setting;
    [SerializeField] Button btn_resume;
    [SerializeField] Button btn_lobby;
    [SerializeField] Button btn_exitGame;

    
    protected override void Init()
    {
        btn_setting.onClick .AddListener( GamePlayManager.Instance.OepnSettingPanel );
        btn_resume.onClick  .AddListener( GamePlayManager.Instance.ClosePlayerInfoPanel );
        btn_lobby.onClick   .AddListener( ()=>{SceneLoadManager.Instance.Load_Lobby();GameManager.Instance.userData.SetInitializationWaitingState();   });
        btn_exitGame.onClick.AddListener( GameManager.Instance.QuitGame);

    }

    protected override void OnOpen()
    {
        // playerEquipmentsUI.UpdateItems();   
        playerSkillsUI.UpdateItems();
    }

    protected override void OnClose()
    {
        
    }

    



}
