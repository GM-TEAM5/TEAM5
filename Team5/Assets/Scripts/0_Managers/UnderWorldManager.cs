using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임플레이매니저와 동격
/// </summary>
public class UnderWorldManager : Singleton<UnderWorldManager>
{
    public static bool isGamePlaying;
    
    // Start is called before the first frame update
    void Start()
    {
        isGamePlaying = false;
        OnEnterUnderWorld();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //==========================
    void OnEnterUnderWorld()
    {
        UnderWorldPlayer.Instance.InitPlayer();
        
        StartCoroutine(EnterSequence());
    }

    IEnumerator EnterSequence()
    {
        DirectingManager.Instance.ZoomIn(UnderWorldPlayer.Instance.t_player); 
        // 이때 플레이어 생성 애니메이션 재생할거임
        yield return new WaitForSeconds(2f);
        DirectingManager.Instance.ZoomOut();
        //
        yield return new WaitForSeconds(2f);
        isGamePlaying = true;
    }
}
