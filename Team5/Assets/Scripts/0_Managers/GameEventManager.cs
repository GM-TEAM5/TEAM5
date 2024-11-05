using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameEventManager : Singleton<GameEventManager>
{
    public UnityEvent onStageLoad = new();
    public UnityEvent onLevelUp = new();

    public UnityEvent onGameOver= new();

    public UnityEvent<Enemy> onEnemyDie = new();    // Enemy : 죽은 에너미


    public UnityEvent<int> onWaveClear = new();     // int : 클리어한 웨이브 번호 

    public UnityEvent<bool> onCloseTo_selectableItemList = new();   // bool :  enter - true, exit - false
    public UnityEvent<SelectableItem> onUpdate_closestSelectableItem = new(); // 가장 가까이 있는 아이템이 달라졌을때,

}
