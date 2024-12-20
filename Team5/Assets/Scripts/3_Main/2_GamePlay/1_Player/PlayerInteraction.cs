using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// '상호작용' 키를 통해 상호작용할 수 있는 게임 오브젝트들.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] List<InteractiveObject> inspectingObjects = new();
    [SerializeField] InteractiveObject closestObject;


    //=====================================================================================
    public void OnUpdate()
    {
        UpdateClosesetObject();

        // 상호작용 
        if( closestObject !=null && Player.Instance.isStunned == false)
        {
            if ( PlayerInputManager.Instance.interact )
            {
                InteractWith(closestObject);
            }
            // else if(PlayerInputManager.Instance.secondaryInteract )
            // {
            //     SecondaryInteractWith(closestObject);
            // }
        }


        UpdateObjectStates();
    }



    void OnTriggerEnter(Collider other)
    {
        // 상호작영 가능한 오브젝트들. 
        if (other.CompareTag("Interactive"))
        {
            InteractiveObject io = other.GetComponent<InteractiveObject>();

            inspectingObjects.Add(io);
        }

    }

    void OnTriggerExit(Collider other)
    {
        // 상호작영 가능한 오브젝트들. 
        if (other.CompareTag("Interactive"))
        {
            InteractiveObject io = other.GetComponent<InteractiveObject>();

            TurnAwayFrom_Interactive( io );    
        }
    }



    //=========================================================================
    /// <summary>
    /// 상호작용 오브젝트가 여러개가 있으면, 이 중에서 가장 가까운 오브젝트를 설정해놓는다. 
    /// </summary>
    void UpdateClosesetObject()
    {
        // if (inspectingObjects.Count == 0)
        // {
        //     if (closestObject !=null)
        //     {
        //         GameEventManager.Instance.onUpdate_inspectingObject.Invoke(null);
        //         closestObject = null;
        //     }
        //     return;
        // }
        
        InteractiveObject newClosestObject = null;
        float sqrDist_old =  Mathf.Infinity;

        Vector3 playerPos = Player.Instance.t.position;
        
        for(int i=0;i<inspectingObjects.Count;i++)
        {
            InteractiveObject interactiveObject = inspectingObjects[i];
            float sqrDist_new = (interactiveObject.transform.position - playerPos).sqrMagnitude;

            if (sqrDist_new < sqrDist_old )
            {
                newClosestObject = interactiveObject;
                sqrDist_old = sqrDist_new;
            }
        }

        if( closestObject != newClosestObject)
        {
            closestObject = newClosestObject;
            Inspect(closestObject);
            GameEventManager.Instance.onUpdate_inspectingObject.Invoke(closestObject);
        }
    }

    /// <summary>
    /// 사용한 것과 조사중이지 않은 오브젝트를 상태 갱신
    /// </summary>
    void UpdateObjectStates()
    {
        for(int i=inspectingObjects.Count-1; i>=0; i--)
        {
            InteractiveObject interactiveObject = inspectingObjects[i];
            
            //
            if ( interactiveObject.isActivated==false )
            {
                TurnAwayFrom_Interactive(interactiveObject);
            }
            else if (interactiveObject != closestObject )
            {
                interactiveObject.OnInspect( false );
            }   
        }
    }


    void Inspect(InteractiveObject interactiveObject)
    {
        if (interactiveObject ==null)
        {
            return;
        }
        
        interactiveObject.OnInspect(true); 
    }


    void TurnAwayFrom_Interactive(InteractiveObject interactiveObject)
    {
        interactiveObject.OnInspect(false); // 조사 해제 
        
        inspectingObjects.Remove(interactiveObject);
    }


    void InteractWith(InteractiveObject interactiveObject)
    {
        interactiveObject.OnInteract();
    }

    // void SecondaryInteractWith(InteractiveObject interactiveObject)
    // {
    //     interactiveObject.OnSecondaryInteract();
    // }

}
