using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameEventManager : Singleton<GameEventManager>
{
    public UnityEvent onStageLoad = new();
    public UnityEvent onLevelUp = new();

    public UnityEvent onGameOver= new();

}
