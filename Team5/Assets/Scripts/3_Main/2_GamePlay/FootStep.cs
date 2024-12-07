using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    public void OnGround()
    {
        if(Player.Instance.stunned == false)
        {
            SoundManager.Instance.Invoke(Player.Instance.t, SoundEventType.PlayerMove);
        }
        
    }
}
