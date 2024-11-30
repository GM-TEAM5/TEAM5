using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WaveActivationSwitch : InteractiveObject
{
    [SerializeField] TextMeshPro text;

    public override bool hasSecondaryInteraction => false;

    protected override void OnInspect_Custom(bool isOn)
    {
        text.gameObject.SetActive(isOn);
    }

    protected override void OnInteract_Custom()
    {
        Debug.Log("웨이브 활성화");

        // GamePlayManager.Instance.StartWave();

        Deactivate();
    }

    //=====================================

    public void OnWaveClear()
    {
        Activate();
    }

    protected override void OnSecondaryInteract_Custom()
    {
        
    }
}
