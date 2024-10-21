using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public abstract class DropItemDataSO : ScriptableObject
{
    public string id;
    public string itemName;
    public Sprite sprite; 

    public abstract void PickUp(float value);
}
