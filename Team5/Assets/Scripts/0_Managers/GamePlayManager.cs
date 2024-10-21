using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : Singleton<GamePlayManager>
{
    [SerializeField] ReinforcementPanel reinforcementPanel; //레벨업 시 강화 패널
    
    //=======================================================================================

    void Start()
    {
        GameEventManager.Instance.onLevelUp.AddListener(OnLevelUp);
        
        StageManager.Instance.Init(TestManager.Instance.testStageData); 
    }


    //========================================

    public void OnLevelUp()
    {
        // 선택지 띄우기
        reinforcementPanel.Open();
        PauseGamePlay(true);
    }
    
    public void OnSelect_ReinforcementOption()
    {
        reinforcementPanel.Close();
        PauseGamePlay(false);
    }



    public void PauseGamePlay(bool pause)
    {
        
        if(pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1f;
        }
        
    }
}
