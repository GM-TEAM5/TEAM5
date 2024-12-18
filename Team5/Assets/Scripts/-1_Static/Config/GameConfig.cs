using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "SO/Config/Game", order = int.MaxValue)]
public class GameConfig : ScriptableObject
{
    public int maxChapter;
}
