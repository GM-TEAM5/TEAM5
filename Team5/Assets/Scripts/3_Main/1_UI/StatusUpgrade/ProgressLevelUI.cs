using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public enum PlayerStatusUpgradeProgressState
{
    Unvalid,
    Completed,
    Selected,
}

public class ProgressLevelUI : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerStatusUpgradeProgressState currState;
    Image img;

    [SerializeField] SerializableDictionary<PlayerStatusUpgradeProgressState, Sprite> dic_sprite;
    [SerializeField] SerializableDictionary<PlayerStatusUpgradeProgressState, Color> dic_color;


    //===========================================

    public void Init()
    {
        img = GetComponent<Image>();

    }

    public void SetState(PlayerStatusUpgradeProgressState targetState)
    {
        currState = targetState;


        img.sprite = dic_sprite[currState];
        img.color = dic_color[currState];
    }
}
