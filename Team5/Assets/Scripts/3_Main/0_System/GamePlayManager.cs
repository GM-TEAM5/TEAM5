using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Threading;
using System.ComponentModel;
using UnityEngine.Rendering;
using JetBrains.Annotations;

/// <summary>
/// 메인 씬의 게임 플레이 로직 및 ui를 관리
/// </summary>
public class GamePlayManager : Singleton<GamePlayManager>
{
    // public static bool isPaused;
    public static bool isGamePlaying = false;
    public bool isStageFinished = false;
    public float gameStartTime;    // 해당 스테이지 시작 시간.
    public float gamePlayTime;
    public List<float> alarms = new();

    //
    [SerializeField] AudioSource bgm;
    // [SerializeField] GameObject panelBackground;
    [SerializeField] EntrancePortal entrancePortal;

    [Header("UI Panels")]
    [SerializeField] GamePlayStartUI gamePlayStartUI;    // 스테이지 시작시 안내창
    [SerializeField] PlayerInfoPanel playerInfoPanel; // 플레이어 정보 패널 - esc 눌렀을 때,
    [SerializeField] EquipmentChangePanel equipmentChangePanel; // 장비 교체 패널 - 장비칸 없었을 때 아이템 먹었을 떄, 
    [SerializeField] GamePlaySettingPanel settingPanel;

    //
    [SerializeField] PlayerStatusUpgradePanel playerStatusUpgradePanel;
    [SerializeField] PlayerSkillUpgradePanel skillUpgradePanel;   //게임오버 패널
    [SerializeField] GameOverPanel gameOverPanel;   //게임오버 패널
    [SerializeField] StageClearUI stageClearUI;   //게임오버 패널



    [Header("etc")]
    // [SerializeField] WaveActivationSwitch waveActivationSwitch;
    // [SerializeField] SelectableItemList selectableItemList;

    [SerializeField] EnemyHpSlider enemyHpSlider;

    // [SerializeField] float instantDeathTime = 180;
    // bool instantDeathCalled;


    public int totalEnemySpawnCount;
    public int totalEnemyKillCount;



    //===================================================================================================================================================
    void Start()
    {
        GameManager.Instance.PauseGamePlay(false);
        isGamePlaying = false;
        isStageFinished = false;
        // GameEventManager.Instance.onLevelUp.AddListener(OnLevelUp);

        // GameEventManager.Instance.onEnemyDie.AddListener((Enemy e)=> CheckWaveClear());
        //
        InitUserData();


        StageManager.Instance.Init();
        InitStageObjects(StageManager.Instance.currStage);

        Player.Instance.InitPlayer(GameManager.Instance.userData);
        // TestManager.Instance.SetBoundImage();

        StartCoroutine(StartGamePlaySequence());
    }

    void InitUserData()
    {
        PlayerDataSO userData = GameManager.Instance.userData;

        if (userData.needInitailization)
        {
            Debug.Log("유저 데이터를 초기화 해야함.");
            userData.InitPlayerData();
        }
        else
        {
            Debug.Log("유저 데이터 초기화 필요 없음. ");
        }
    }

    IEnumerator StartGamePlaySequence()
    {
        Sequence startSequence = gamePlayStartUI.GetSeq_GamePlayStart();
        // Sequence generatePortalSeq = entrancePortal.GetSeq_GeneratePortal(1.5f);
        // Sequence playerEnterPortalSeq = Player.Instance.GetSequence_EnterPortal(false, 1f);
        //
        // generatePortalSeq.Play();
        // yield return new WaitUntil( ()=> generatePortalSeq.IsActive()==false );

        // yield return new WaitForSeconds(0.5f);    //대기시간

        // playerEnterPortalSeq.Play();
        // yield return new WaitUntil( ()=> playerEnterPortalSeq.IsActive()==false );

        startSequence.Play();

        yield return new WaitUntil(() => startSequence.IsActive() == false);

        // entrancePortal.PlaySeq_DestroyPortal(2f);


        Player.Instance.OnStartGamePlay();


        gameStartTime = Time.time;
        isGamePlaying = true;
        isStageFinished = true;
        bgm.Play();

        // StartCoroutine( CheckLevelUp() );        
        // StartCoroutine( SetTimer() );       
        StartStage();
    }

    void Update()
    {
        // esc 누르면 상태창나옴.
        if (PlayerInputManager.Instance.flowControl)
        {
            if (playerInfoPanel.gameObject.activeSelf == false)
            {
                OpenPlayerInfoPanel();
            }
            else
            {
                CloseSettingPanel();
                ClosePlayerInfoPanel();
            }


        }


        if( Input.GetKeyDown(KeyCode.Alpha6))
        {
            OpenPlayerStatusUpgradePanel();
        }
        
        
        
        
        if (isGamePlaying == false)
        {
            return;
        }

        gamePlayTime += Time.deltaTime;

        CheckAlarms();
        // if ( gamePlayTime >= instantDeathTime && instantDeathCalled == false)
        // {
        //     instantDeathCalled =true;
        //     TestManager.Instance.KillPlayer();
        // }
    }


    /// 이미 초기화는 끝난 상태. 변수 리디렉션만 함. 
    public void InitStageObjects(Stage stage)
    {
    }

    public void SetAlarm(float time)
    {
        alarms.Add(time);
    }

    void CheckAlarms()
    {
        for (int i = alarms.Count - 1; i >= 0; i--)
        {
            if (gamePlayTime >= alarms[i])
            {
                GameEventManager.Instance.onAlarm.Invoke(alarms[i]);
                alarms.RemoveAt(i);
            }
        }
    }

    //========================================

    public void OpenSettingPanel()
    {
        settingPanel.Open();
        // GameManager.Instance.PauseGamePlay(true);   
    }

    public void CloseSettingPanel()
    {
        settingPanel.Close();
    }


    #region ==== Stage ====


    //--------------------------------------------------
    [Header("Stage")]
    public int targetSpawnCount_currWave;    // 이번 웨이브에서 생성해야할 적의 수 
    public int spawnCount_currWave;          // 이번 웨이브에서 생성한 적의 수 
    public int killCount_currWave;


    // public bool inProgress_waveSpawn =>  spawnCount_currWave != targetSpawnCount_currWave;  //웨이브 스폰이 진행중인지. 
    // public bool inProgress_waveBattle => killCount_currWave <   targetSpawnCount_currWave ; //웨이브가 전투중인지. (생성된 적이 다 안죽었는 지)

    //--------------------------------------------------

    public void StartStage()
    {
        StageManager.Instance.StartStage();
        // GameEventManager.Instance.onStageStart.Invoke( StageManager.Instance.stageWave );
    }


    /// 웨이브를 시작시킨다. 
    public void OnStartWave()
    {
        // targetSpawnCount_currWave = 0;
        // spawnCount_currWave = 0;
        // killCount_currWave = 0;


        // StageManager.Instance.StartWave();

        // selectableItemList.Deactivate();

        // StartCoroutine( CheckWaveClear() );
    }



    /// <summary>
    /// 
    /// </summary>
    public void OnStageClear()
    {
        GameEventManager.Instance.onStageFinish.Invoke();
        StartCoroutine(StageClearSequence());
    }

    IEnumerator StageClearSequence()
    {
        Sequence seq_stageClear = stageClearUI.GetSeq_StageClear();
        seq_stageClear.Play();
        yield return new WaitUntil(() => seq_stageClear.IsActive() == false);


        // OpenUpgradePanel();
    }

    public void OnEnemyKill(Enemy enemy)
    {
        // totalEnemyKillCount++;
        killCount_currWave++;

        GameEventManager.Instance.onEnemyDie.Invoke(enemy);
    }

    public void UpdateEnemyHpSlider(Enemy enemy)
    {
        enemyHpSlider.UpdateHp(enemy);
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
        GameEventManager.Instance.onSelectItem.Invoke();
        // selectableItemInfoPanel.Close();
        // selectableItemList.Deactivate(); 
    }


    public void OnInventoryFull(EquipmentItemSO equipment)
    {
        equipmentChangePanel.Open();
        equipmentChangePanel.InitSelectedEquipment(equipment);
    }

    // public void Reroll(SelectableItem selectableItem)
    // {
    //     if (Player.Instance.status.rerollCount <=0)
    //     {
    //         return;
    //     }
    //     selectableItemList.Reroll(selectableItem);
    //     Player.Instance.status.ChangeRerollCount(-1);
    //     GameEventManager.Instance.onReroll.Invoke(selectableItem);

    // }

    public void Reroll(UpgradeSelection selection)
    {
        if (Player.Instance.status.rerollCount <= 0)
        {
            return;
        }
        //
        Player.Instance.status.ChangeRerollCount(-1);
        skillUpgradePanel.Reroll(selection);
        GameEventManager.Instance.onUpgradeReroll.Invoke();
        //
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
        // reinforcementPanel.Close();
        GameManager.Instance.PauseGamePlay(false);
    }

    #region Upgrade
    public void OpenPlayerStatusUpgradePanel()
    {
        GameManager.Instance.PauseGamePlay(true);
        playerStatusUpgradePanel.Open();
    }

    public void ClosePlayerStatusUpgradePanel()
    {
        playerStatusUpgradePanel.Close();
        GameManager.Instance.PauseGamePlay(false);
    }



    public void OpenSkillUpgradePanel(SkillProperty property)
    {
        skillUpgradePanel.Open(property);
        GameManager.Instance.PauseGamePlay(true);
    }
    public void CloseSkillUpgradePanel()
    {
        skillUpgradePanel.Close();
        GameManager.Instance.PauseGamePlay(false);
    }



    public void OpenPlayerInfoPanel()
    {
        isGamePlaying = false;  // 직접 상태 변경
        playerInfoPanel.Open();
        GameManager.Instance.PauseGamePlay(true);
    }

    public void ClosePlayerInfoPanel()
    {
        isGamePlaying = true;  // 직접 상태 변경
        playerInfoPanel.Close();
        GameManager.Instance.PauseGamePlay(false);
    }

    #endregion


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
        DirectingManager.Instance.ZoomIn(Player.Instance.t);
        GameManager.Instance.PauseGamePlay(true);
        yield return new WaitForSecondsRealtime(1f);
        GameManager.Instance.PauseGamePlay(false, 2f);
        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(DirectingManager.Instance.FadeSequene());
        gameOverPanel.Open();
        DirectingManager.Instance.ZoomOut();
    }


    //====================================================


}
