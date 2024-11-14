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
    // [SerializeField] GameObject panelBackground;
    [SerializeField] EntrancePortal entrancePortal;
    [SerializeField] GamePlayStartUI gamePlayStartUI;    // 스테이지 시작시 안내창
    [SerializeField] ReinforcementPanel reinforcementPanel; //레벨업 시 강화 패널
    [SerializeField] PlayerInfoPanel playerInfoPanel; // 플레이어 정보 패널 - esc 눌렀을 때,
    [SerializeField] EquipmentChangePanel equipmentChangePanel; // 장비 교체 패널 - 장비칸 없었을 때 아이템 먹었을 떄, 

    //
    [SerializeField] SelectableItemInfoPanel selectableItemInfoPanel;   // 웨이브 종료시 나타나는 아이템 설명 팝업창
    [SerializeField] WaveActivationSwitch waveActivationSwitch;
    [SerializeField] SelectableItemList selectableItemList;
    [SerializeField] StagePortal stagePortal;


    [SerializeField] GameOverPanel gameOverPanel;   //게임오버 패널
    [SerializeField] TextMeshProUGUI text_timer;   //게임오버 패널

    [SerializeField] float instantDeathTime = 180;
    bool instantDeathCalled;


    public int totalEnemySpawnCount;
    public int totalEnemyKillCount;



    //===================================================================================================================================================
    void Start()
    {           
        isGamePlaying = false;
        // GameEventManager.Instance.onLevelUp.AddListener(OnLevelUp);

        // GameEventManager.Instance.onEnemyDie.AddListener((Enemy e)=> CheckWaveClear());

        
        Player.Instance.InitPlayer();
        StageManager.Instance.Init(TestManager.Instance.testStageData);     
        InitStageObjects( StageManager.Instance.currStage);
        
        TestManager.Instance.SetBoundImage();
        StartCoroutine( StartGamePlaySequence());
    }

    IEnumerator StartGamePlaySequence()
    {
        // Sequence startSequence = gamePlayStartUI.GetSeq_GamePlayStart();
        // Sequence generatePortalSeq = entrancePortal.GetSeq_GeneratePortal(1.5f);
        // Sequence playerEnterPortalSeq = Player.Instance.GetSequence_EnterPortal(false, 1f);
        //
        // generatePortalSeq.Play();
        // yield return new WaitUntil( ()=> generatePortalSeq.IsActive()==false );

        // yield return new WaitForSeconds(0.5f);    //대기시간

        // playerEnterPortalSeq.Play();
        // yield return new WaitUntil( ()=> playerEnterPortalSeq.IsActive()==false );

        // startSequence.Play();
        // yield return new WaitUntil( ()=>startSequence.IsActive()==false);

        // entrancePortal.PlaySeq_DestroyPortal(2f);
        

        yield return null;

        Player.Instance.OnStartGamePlay();


        gameStartTime = Time.time;
        isGamePlaying = true;
        bgm.Play();

        // StartCoroutine( CheckLevelUp() );        
        // StartCoroutine( SetTimer() );       
    }

    void Update()
    {
        // esc 누르면 상태창나옴.
        if(PlayerInputManager.Instance.pause)
        {
            if(playerInfoPanel.gameObject.activeSelf == false)
            {
                OpenPlayerInfoPanel();
            }
            else
            {
                ClosePlayerInfoPanel();
            }
            
            
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            OnInventoryFull(null);
        }
        
        
        
        
        if (isGamePlaying == false)
        {
            return;
        }

        gamePlayTime += Time.deltaTime;
        // if ( gamePlayTime >= instantDeathTime && instantDeathCalled == false)
        // {
        //     instantDeathCalled =true;
        //     TestManager.Instance.KillPlayer();
        // }
    }


    /// 이미 초기화는 끝난 상태. 변수 리디렉션만 함. 
    public void InitStageObjects(Stage stage)
    {
        selectableItemInfoPanel.ForceInit();
        
        waveActivationSwitch = stage.waveActivationSwitch;
        selectableItemList = stage.selectableItemList;
        stagePortal = stage.stagePortal;
    }

    //========================================

    #region ==== Stage ====


    //--------------------------------------------------
    [Header("Stage")]
    public int targetSpawnCount_currWave;    // 이번 웨이브에서 생성해야할 적의 수 
    public int spawnCount_currWave;          // 이번 웨이브에서 생성한 적의 수 
    public int killCount_currWave;


    public bool inProgress_waveSpawn =>  spawnCount_currWave != targetSpawnCount_currWave;  //웨이브 스폰이 진행중인지. 
    public bool inProgress_waveBattle => killCount_currWave <   targetSpawnCount_currWave ; //웨이브가 전투중인지. (생성된 적이 다 안죽었는 지)

    //--------------------------------------------------

    public void StartStage()
    {

    }


    /// 웨이브를 시작시킨다. 
    public void StartWave()
    {
        targetSpawnCount_currWave = 0;
        spawnCount_currWave = 0;
        killCount_currWave = 0;
        
        
        StageManager.Instance.StartWave();
        
        selectableItemList.Deactivate();

        StartCoroutine( CheckWaveClear() );
    }

    /// <summary>
    /// 웨이브 종료를 감지하고, 종료 후엔 그에 맞는 처리를 한다. 
    /// </summary>
    /// <returns></returns>
    public IEnumerator CheckWaveClear()
    {
        // 
        yield return new WaitUntil( ()=> inProgress_waveSpawn == false && inProgress_waveBattle == false);      // 웨이브가 종료될 때 까지 기다림. 
    
        ClearWave();        // 웨이브 클리어 처리. 

        if (StageManager.Instance.IsStageClear())
        {
            ClearStage();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ClearWave()
    {
        StageManager.Instance.OnWaveClear();

        selectableItemList.OnWaveClear(); 
        waveActivationSwitch.OnWaveClear();
    }
    
    
    /// <summary>
    /// 
    /// </summary>
    public void ClearStage()
    {
        stagePortal.OnStageClear();

        StageManager.Instance.OnStageClear();
    }


    #endregion
    //==========================================================
    #region Selectable Items

    public void Select_SelectableItem(ItemDataSO item)
    {
        // Debug.Log($"선택띠 : {selectableItem.data.id}, {selectableItem.data.dataName}");
        if (item.TryGet())
        {
            FinishSelection();
        }
    }

    public void FinishSelection()
    {
        selectableItemInfoPanel.Close();
        selectableItemList.Deactivate(); 
    }


    public void OnInventoryFull(EquipmentItemSO equipment)
    {
        equipmentChangePanel.Open();
        equipmentChangePanel.InitSelectedEquipment(equipment);
    }

    #endregion

    /// <summary>
    /// 게임 진행 중 강화 선택지 창을 연다.  - 레벨업을 하여 강화를 해야할 때, 
    /// </summary>
    /// <returns></returns>
    // IEnumerator CheckLevelUp()
    // {
    //     var waitForSeconds = new WaitForSeconds(0.25f);
    //     while( isGamePlaying )
    //     {
    //         bool isAvailable = Player.Instance.reinforcementLevel < Player.Instance.status.level;
            
    //         if (reinforcementPanel.gameObject.activeSelf == false  &&  isAvailable )
    //         {
    //             reinforcementPanel.Open();
    //             GameManager.Instance.PauseGamePlay(true);
    //         } 
            
    //         yield return waitForSeconds;
    //     }

    // }

    /// <summary>
    ///  강화 선택지를 누르면 창을 닫고 resume
    /// </summary>
    public void OnSelect_ReinforcementOption()
    {
        reinforcementPanel.Close();
        GameManager.Instance.PauseGamePlay(false);
    }



    public void OpenPlayerInfoPanel()
    {
        playerInfoPanel.Open();
        GameManager.Instance.PauseGamePlay(true);   
    }

    public void ClosePlayerInfoPanel()
    {
        playerInfoPanel.Close();
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
    



    // IEnumerator SetTimer()
    // {
    //     var waitForSeconds = new WaitForSeconds(1f);
    //     while( isGamePlaying )
    //     {
            
    //         //
    //         int mins = (int)gamePlayTime/60;
    //         int secs = (int)gamePlayTime % 60;
        
    //         text_timer.SetText($"{mins:00}:{secs:00}");
            
    //         yield return waitForSeconds;
    //     }
        

    // }



}
