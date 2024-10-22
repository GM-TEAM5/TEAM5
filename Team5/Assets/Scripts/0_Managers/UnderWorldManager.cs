using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderWorldManager : Singleton<UnderWorldManager>
{
    // Start is called before the first frame update
    void Start()
    {
        OnEnterUnderWorld();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //==========================
    void OnEnterUnderWorld()
    {
        Player.Instance.InitPlayer();
        
        StartCoroutine(EnterSequence());
    }

    IEnumerator EnterSequence()
    {
        DirectingManager.Instance.ZoomIn(Player.Instance.t_player);
        yield return new WaitForSeconds(2f);
        DirectingManager.Instance.ZoomOut();
    }
}
