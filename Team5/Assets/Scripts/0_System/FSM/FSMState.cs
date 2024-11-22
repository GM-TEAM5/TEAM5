using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class FSMState
{
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();    
}
