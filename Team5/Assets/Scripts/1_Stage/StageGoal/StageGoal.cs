using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum StageGoalType
{
    Survival,
    Elimination
}

[Serializable]
public abstract class StageGoal : MonoBehaviour
{
    public abstract StageGoalType type {get;}
    
    public abstract bool IsStageClear();



}
