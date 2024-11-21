using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameEventManager : Singleton<GameEventManager>
{
    public UnityEvent onInitPlayer = new();   // 메인씬 로드 직후, 필요 데이터가 초기화 된 후
    public UnityEvent<KeyCode,PlayerSkill> onChangeSkill = new();   // 스킬을 변경했을 때,  ->  KeyCode : ,PlayerSKill 
    
    
    public UnityEvent onStageLoad = new();
    public UnityEvent onLevelUp = new();

    public UnityEvent onGameOver= new();

    public UnityEvent<Enemy> onEnemyDie = new();    // Enemy : 죽은 에너미


    public UnityEvent<int> onWaveClear = new();     // int : 클리어한 웨이브 번호 

    public UnityEvent<bool> onCloseTo_selectableItemList = new();   // bool :  enter - true, exit - false
    public UnityEvent<SelectableItem> onReroll = new();  
    public UnityEvent onUpgradeReroll = new();   
    public UnityEvent onSelectItem;
 
    public UnityEvent<InteractiveObject> onUpdate_inspectingObject = new();   
    public UnityEvent<ItemDataSO> onUpdate_closestSelectableItem = new(); // 가장 가까이 있는 아이템이 달라졌을때,


    #region 스탯 변동 
    public UnityEvent onChangePlayerStatus_maxHp;
    public UnityEvent onChangePlayerStatus_pDmg;
    #endregion
    

}
