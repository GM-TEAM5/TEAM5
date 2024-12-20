using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageReward : InteractiveObject
{
    [SerializeField] SpriteRenderer sr_reward;
    [SerializeField] TextMeshPro text;
    public StageRewardSO data;
    

    //========================================================================
    protected override void OnInspect_Custom(bool isOn)
    {
        text.gameObject.SetActive(isOn);
    }

    protected override void OnInteract_Custom()
    {
        
    }

    //========================================================================
    public void Init(StageRewardSO data)
    {
        this.data = data;
        sr_reward.sprite = data.sprite;
    }
}
