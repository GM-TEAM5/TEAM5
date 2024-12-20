using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class GameOverPanel : GamePlayPanel 
{
    [SerializeField] GameObject coms;   // 패널 구성요소들
    [SerializeField] Button btn_ok;
    // [SerializeField] Image img_panel;   // 패널자체의 이미지
    
    //===================================
    
    protected override void Init()
    {
        // img_panel = GetComponent<Image>();
        
        btn_ok.onClick.AddListener(GoToUnderWorld);
    }
    
    protected override void OnOpen()
    {
        SetStatistics();
    }

    protected override void OnClose()
    {

    }

    //===================================

    void SetStatistics()
    {

    }

    void GoToUnderWorld()
    {
        GameManager.Instance.userData.SetInitializationWaitingState();
        SceneLoadManager.Instance.Load_UnderWorld();
    }

}
