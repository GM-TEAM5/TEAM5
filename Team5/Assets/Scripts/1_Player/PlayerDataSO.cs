using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "SO/Player/PlayerData", order = int.MaxValue)] 
public class PlayerDataSO : ScriptableObject
{
    public int traitPoint;
    public int currChapter = 1;
    public int currStage = 1;
    public int currStagePlayCount;
    public int deathCount;
    

    public bool isNewUser => deathCount==0 && currChapter ==1 && currStage ==1;
}
