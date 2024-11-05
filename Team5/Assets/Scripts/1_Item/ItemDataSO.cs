using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemDataSO : GameData 
{
    public Sprite sprite; 
    public int tier;

    public abstract void Get();
}
