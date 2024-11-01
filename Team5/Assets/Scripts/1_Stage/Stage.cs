using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    
    Transform t_playerSpawnPoint;
    MeshCollider[] enemySpawnArea;


    public Vector3 GetRandomSpawnPoint()
    {
        Vector3 ret = Player.Instance.t_player.position;

        return ret;
    }

}
