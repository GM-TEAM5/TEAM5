using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum으로 사용하기 위함. - 어떤 형태로 적을 생성할지. 
/// </summary>
public abstract class WaveFormSO : ScriptableObject
{
    //    
    public abstract void Spawn(List<string> enemyIds, Vector3 spawnPoint);
}
