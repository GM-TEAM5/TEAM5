using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDeath : MonoBehaviour
{

    void Update()
    {
        if(Player.Instance.isAlive== false)
        {
            return;
        }
        Vector3 playerPos = Player.Instance.t_player.position;
        transform.position = new Vector3(playerPos.x, transform.position.y, playerPos.z-1);
        transform.position += Vector3.down * 50 * Time.deltaTime; 

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("즉사");
            Player.Instance.GetDamaged(999999);
        }

    }

}
