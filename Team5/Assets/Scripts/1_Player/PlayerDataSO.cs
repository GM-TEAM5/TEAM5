using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "SO/Player/PlayerData", order = int.MaxValue)] 
public class PlayerDataSO : ScriptableObject
{
    public int traitPoint;
    public int currChapter;
    public int currStage;
    public int currStagePlayCount;
}
