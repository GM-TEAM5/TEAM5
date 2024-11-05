using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(SphereCollider))]
public abstract class InteractableObject : MonoBehaviour
{
    Collider _collider;
    [SerializeField] protected bool isPlayerInRange;
    [SerializeField] protected bool locked;

    


    protected virtual void Start()
    {
        _collider = GetComponent<Collider>();
        OnEnter(false);

        GetComponent<SpriteEntity>()?.Init();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            // Debug.Log("On");
            OnEnter(isPlayerInRange);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // Debug.Log("Off");
            OnEnter(isPlayerInRange);
        }
    }

    protected virtual void Update()
    {
        if (locked==false && isPlayerInRange && PlayerInputManager.Instance.isInteractOn)
        {
            Deactivate();
            OnInteract();
        }
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
        
        OnEnter(false);
        GetComponent<Collider>().enabled = false;
    }

    
    protected abstract void OnEnter(bool isOn);

    protected abstract void OnInteract();

}
