using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// [RequireComponent(typeof(SphereCollider))]
public abstract class InteractiveObject : MonoBehaviour
{

    // [SerializeField] protected bool isPlayerInRange;
    public bool isActivated =>locked ==false && gameObject.activeSelf;
    
    public bool locked ;

    protected bool beingInspected; // 플레이어가 현재 바라보고 있는지;

    // public abstract bool hasSecondaryInteraction {get;} 

    void Awake()
    {
        GetComponent<SpriteEntity>()?.Init();

        OnInspect_Custom(false);

    }

    //==============================================================================
    
    protected void Activate()
    {
        locked = false;
        GetComponent<Collider>().enabled = true;
    }

    protected void Deactivate()
    {
        locked = true;
        GetComponent<Collider>().enabled = false;

        //
        OnInspect(false);
    }

    public void OnInspect(bool isOn)
    {
        if (beingInspected == isOn)
        {
            return;
        }
        beingInspected = isOn;

        OnInspect_Custom(isOn);   
    }


    protected abstract void OnInspect_Custom(bool isOn);


    public void OnInteract()
    {
        OnInteract_Custom();
    }

    // public void OnSecondaryInteract()
    // {
    //     if (hasSecondaryInteraction == false)
    //     {
    //         return;
    //     }
    //     OnSecondaryInteract_Custom();
    // }

    protected abstract void OnInteract_Custom();

    // protected abstract void OnSecondaryInteract_Custom();
}
