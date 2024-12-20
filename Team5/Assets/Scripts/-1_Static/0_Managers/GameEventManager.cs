using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameEventManager : Singleton<GameEventManager>
{
    public UnityEvent onInitPlayer = new();   // 메인씬 로드 직후, 필요 데이터가 초기화 된 후
    public UnityEvent<KeyCode,PlayerSkill> onChangeSkill = new();   // 스킬을 변경했을 때,  ->  KeyCode : ,PlayerSKill 
    
    public UnityEvent<float> onAlarm = new();
    
    public UnityEvent onStageLoad = new();
    // public UnityEvent onLevelUp = new();

    public UnityEvent onGameOver= new();

    public UnityEvent<StageWave> onStageStart=new();
    public UnityEvent onStageFinish = new();
    public UnityEvent<Enemy> onEnemyDie = new();    // Enemy : 죽은 에너미


    public UnityEvent<int> onWaveClear = new();     // int : 클리어한 웨이브 번호 

    // public UnityEvent<bool> onCloseTo_selectableItemList = new();   // bool :  enter - true, exit - false
    // public UnityEvent<SelectableItem> onReroll = new();  
    public UnityEvent onUpgradeReroll = new();   
    public UnityEvent onSelectItem;
 
    public UnityEvent<InteractiveObject> onUpdate_inspectingObject = new();   
    // public UnityEvent<ItemDataSO> onUpdate_closestSelectableItem = new(); // 가장 가까이 있는 아이템이 달라졌을때,


    #region 스탯 변동 
    // ui 에 반영될 때 호출 필요. 
    public UnityEvent onChangePlayerStatus_maxHp = new();   // 
    public UnityEvent onChangePlayerStatus_maxInk = new();  //
    // public UnityEvent onChangePlayerStatus_pDmg = new();
    // public UnityEvent onChangePlayerStatus_mDmg = new();

    public UnityEvent<int,int> onChangePlayerGold = new();  // int 1: 변동량 , int 2 : 변동 후 골드량
    #endregion
    

    public void OnSceneChanged()
    {

    }

    void Awake()
    {

    }

    void Start()
    {

    }

}
