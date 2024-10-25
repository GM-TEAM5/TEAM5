using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            UnderWorldManager.Instance.LeaveUnderWorld();
            GetComponent<Collider>().enabled = false;
        }
    }
}
