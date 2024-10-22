using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class GamePlayManager : Singleton<GamePlayManager>
{
    public static bool isGamePlaying = false;
    
    
    
    // [SerializeField] int reinforcementLevel;    // 강화패널 레벨 ( 상점 레벨 ) - 플레이어 레벨을 따라가며, 레벨이 높을 수록 더 좋은 선택지가 나옴. 나중에 따로 스크립트 뺄거임. 
    
    [SerializeField] ReinforcementPanel reinforcementPanel; //레벨업 시 강화 패널


    [SerializeField] GameOverPanel gameOverPanel;

    
    //=======================================================================================

    void Start()
    {           
        GameEventManager.Instance.onLevelUp.AddListener(OnLevelUp);
        
        StageManager.Instance.Init(TestManager.Instance.testStageData); 
        

        // gameStart;
        isGamePlaying = true;
    }


    //========================================

    public void OnLevelUp()
    {
        DOTween.Sequence()
        .AppendInterval(0.5f) // 조금의 딜레이~
        .AppendCallback( ()=> {
             if (reinforcementPanel.gameObject.activeSelf==false)
            {
                reinforcementPanel.Open();
                PauseGamePlay(true);
            } })
        .Play(); 
    }

    
    public void OnSelect_ReinforcementOption()
    {
        reinforcementPanel.Close();
        PauseGamePlay(false);

        if(Player.Instance.reinforcementLevel < Player.Instance.status.level)
        {
            OnLevelUp();
        }
    }


    //========================================
    public void GameOver()
    {
        // 해당 함수가 여러번 실행되지 않게. 
        if (isGamePlaying == false)
        {
            return;
        }
        
        Debug.Log("----게임오버 ------");
        isGamePlaying = false;

        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        DirectingManager.Instance.ZoomIn(Player.Instance.t_player);
        PauseGamePlay(true);
        yield return new WaitForSecondsRealtime(1.5f);
        yield return StartCoroutine(DirectingManager.Instance.FadeSequene() );
        gameOverPanel.Open();
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
