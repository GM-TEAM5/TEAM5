using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    public void OnGround()
    {
        if(Player.Instance.stunned == false)
        {
            TestManager.Instance.TestSFX_FootStep();
        }
        
    }
}
