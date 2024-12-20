using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTier
{
    // Normal,
    Rare,
    Epic,
    Unique,
    // Legendary
}

public enum ItemType
{
    Consumable,
    Equipment,
    Skill
}

public enum CantGetReason
{
    None,
    NoSpace
}



public abstract class ItemDataSO : GameData 
{
    // public Sprite sprite; 
    public ItemType type;
    public ItemTier tier;

    [TextArea(3, 10)]  public string description;

    public bool TryGet()
    {
        if (CanGet(out CantGetReason reason))
        {
            Get();
            return true;
        }
        else
        {
            OnCantGet(reason);
            return false;
        }
    }
    
    protected abstract bool CanGet(out CantGetReason reason);
    
    protected abstract void Get();

    protected abstract void OnCantGet(CantGetReason reason);

    

}
