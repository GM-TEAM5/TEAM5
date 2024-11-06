using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WaveActivationSwitch : InteractableObject
{
    [SerializeField] TextMeshPro text;
    
    

    protected override void OnEnter(bool isOn)
    {
        text.gameObject.SetActive(isOn);
    }

    protected override void OnInteract()
    {
        Debug.Log("웨이브 활성화");

        GamePlayManager.Instance.StartWave();
    }

    public void OnWaveClear()
    {
        Activate();
    }

}
