using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Threading;

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
    [SerializeField] AudioSource bgm;
    [SerializeField] EntrancePortal entrancePortal;
    [SerializeField] GamePlayStartUI gamePlayStartUI;    // 스테이지 시작시 안내창
    [SerializeField] ReinforcementPanel reinforcementPanel; //레벨업 시 강화 패널
    [SerializeField] GameOverPanel gameOverPanel;   //게임오버 패널
    [SerializeField] TextMeshProUGUI text_timer;   //게임오버 패널

    bool instantDeathCalled;

    
    //=======================================================================================
    void Start()
    {           
        isGamePlaying = false;
        // GameEventManager.Instance.onLevelUp.AddListener(OnLevelUp);

        StageManager.Instance.Init(TestManager.Instance.testStageData);     
        TestManager.Instance.SetBoundImage();

        Player.Instance.InitPlayer();
        StartCoroutine( StartGamePlaySequence());
    }

    IEnumerator StartGamePlaySequence()
    {
        Sequence startSequence = gamePlayStartUI.GetSeq_GamePlayStart();
        Sequence generatePortalSeq = entrancePortal.GetSeq_GeneratePortal(1.5f);
        Sequence playerEnterPortalSeq = Player.Instance.GetSequence_EnterPortal(false, 1f);
        //
        generatePortalSeq.Play();
        yield return new WaitUntil( ()=> generatePortalSeq.IsActive()==false );

        yield return new WaitForSeconds(0.5f);    //대기시간

        playerEnterPortalSeq.Play();
        yield return new WaitUntil( ()=> playerEnterPortalSeq.IsActive()==false );

        startSequence.Play();
        yield return new WaitUntil( ()=>startSequence.IsActive()==false);

        entrancePortal.PlaySeq_DestroyPortal(2f);
        

        Player.Instance.OnStartGamePlay();


        gameStartTime = Time.time;
        isGamePlaying = true;
        bgm.Play();

        StartCoroutine( CheckLevelUp() );        
        StartCoroutine( SetTimer() );       
        
        StageManager.Instance.OnStartGamePlay();
    }

    void Update()
    {
        if (isGamePlaying == false)
        {
            return;
        }

        gamePlayTime += Time.deltaTime;

        if ( gamePlayTime > 5 && instantDeathCalled == false)
        {
            instantDeathCalled =true;
            TestManager.Instance.KillPlayer();
        }
    }

    //========================================

    /// <summary>
    /// 게임 진행 중 강화 선택지 창을 연다.  - 레벨업을 하여 강화를 해야할 때, 
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckLevelUp()
    {
        var waitForSeconds = new WaitForSeconds(0.25f);
        while( isGamePlaying )
        {
            bool isAvailable = Player.Instance.reinforcementLevel < Player.Instance.status.level;
            
            if (reinforcementPanel.gameObject.activeSelf == false  &&  isAvailable )
            {
                reinforcementPanel.Open();
                GameManager.Instance.PauseGamePlay(true);
            } 
            
            yield return waitForSeconds;
        }

    }

    /// <summary>
    ///  강화 선택지를 누르면 창을 닫고 resume
    /// </summary>
    public void OnSelect_ReinforcementOption()
    {
        reinforcementPanel.Close();
        GameManager.Instance.PauseGamePlay(false);
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
        DirectingManager.Instance.ZoomOut();
    }


    //====================================================
    IEnumerator SetTimer()
    {
        var waitForSeconds = new WaitForSeconds(1f);
        while( isGamePlaying )
        {
            
            //
            int mins = (int)gamePlayTime/60;
            int secs = (int)gamePlayTime % 60;
        
            text_timer.SetText($"{mins:00}:{secs:00}");
            
            yield return waitForSeconds;
        }
        

    }



}
