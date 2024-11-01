using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class InteractableObject : MonoBehaviour
{
    SphereCollider sphereCollider;
    bool isPlayerInRange;
    [SerializeField] protected bool locked;

    


    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        OnEnter(false);

        GetComponent<SpriteEntity>().Init();
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

    void Update()
    {
        if (locked==false && isPlayerInRange && PlayerInputManager.Instance.isInteractOn)
        {
            OnInteract();
        }
    }

    //==============================================================================
    
    protected abstract void OnEnter(bool isOn);

    protected abstract void OnInteract();
}
