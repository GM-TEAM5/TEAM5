using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : Singleton<GamePlayManager>
{
    [SerializeField] int reinforcementLevel;    // 강화패널 레벨 ( 상점 레벨 ) - 플레이어 레벨을 따라가며, 레벨이 높을 수록 더 좋은 선택지가 나옴. 나중에 따로 스크립트 뺄거임. 
    
    [SerializeField] ReinforcementPanel reinforcementPanel; //레벨업 시 강화 패널
    
    //=======================================================================================

    void Start()
    {
        GameEventManager.Instance.onLevelUp.AddListener(OnLevelUp);
        
        StageManager.Instance.Init(TestManager.Instance.testStageData); 

        reinforcementLevel = Player.Instance.status.level;  
    }


    //========================================

    public void OnLevelUp()
    {
        // 선택지 띄우기
        reinforcementLevel++;
        reinforcementPanel.Open(reinforcementLevel);
        PauseGamePlay(true);
    }
    
    public void OnSelect_ReinforcementOption()
    {
        reinforcementPanel.Close();
        PauseGamePlay(false);

        if(reinforcementLevel < Player.Instance.status.level)
        {
            OnLevelUp();
        }
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
