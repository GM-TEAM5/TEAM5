using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 메인 씬의 게임 플레이 로직 및 ui를 관리
/// </summary>
public class GamePlayManager : Singleton<GamePlayManager>
{
    // public static bool isPaused;
    public static bool isGamePlaying = false;
    public float gameStartTime;    // 해당 스테이지 시작 시간.
    public float gamePlayTime; 
    
    //
    [SerializeField] GamePlayStartUI gamePlayStartUI;    // 스테이지 시작시 안내창
    [SerializeField] ReinforcementPanel reinforcementPanel; //레벨업 시 강화 패널
    [SerializeField] GameOverPanel gameOverPanel;   //게임오버 패널

    
    //=======================================================================================
    void Start()
    {           
        isGamePlaying = false;
        GameEventManager.Instance.onLevelUp.AddListener(OnLevelUp);
        
        StageManager.Instance.Init(TestManager.Instance.testStageData);     
        
        StartCoroutine( StartGamePlaySequence());
    }

    IEnumerator StartGamePlaySequence()
    {
        Sequence startSequence = gamePlayStartUI.StartGamePlaySequence();
        yield return new WaitUntil( ()=>startSequence.IsActive()==false);

        gameStartTime = Time.time;
        isGamePlaying = true;
        
        StageManager.Instance.OnStartGamePlay();
    }

    void Update()
    {
        if (isGamePlaying == false)
        {
            return;
        }

        gamePlayTime += Time.deltaTime;
    }

    //========================================

    public void OnLevelUp()
    {
        DOTween.Sequence()
        .AppendInterval(0.5f) // 조금의 딜레이~ 이것때문에 지금 버그 생기는중. 
        .AppendCallback( ()=> {
             if (reinforcementPanel.gameObject.activeSelf==false)
            {
                reinforcementPanel.Open();
                GameManager.Instance.PauseGamePlay(true);
            } })
        .Play(); 
    }

    
    public void OnSelect_ReinforcementOption()
    {
        reinforcementPanel.Close();
        GameManager.Instance.PauseGamePlay(false);

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
        GameEventManager.Instance.onGameOver.Invoke();
        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        DirectingManager.Instance.ZoomIn(Player.Instance.t_player);
        GameManager.Instance.PauseGamePlay(true);
        yield return new WaitForSecondsRealtime(1f);
        GameManager.Instance.PauseGamePlay(false,2f);
        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(DirectingManager.Instance.FadeSequene() );
        gameOverPanel.Open();
    }


}
