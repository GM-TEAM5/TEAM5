using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tier
{
    Normal,
    Rare,
    Epic,
    Unique,
    Legendary
}


public abstract class ItemDataSO : GameData 
{
    public Sprite sprite; 
    public Tier tier;

    [TextArea(3, 10)]  public string description;

    public abstract void Get();

}
