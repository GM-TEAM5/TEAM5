using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 게임플레이매니저와 동격
/// </summary>
public class UnderWorldManager : Singleton<UnderWorldManager>
{
    public static bool isGamePlaying;
    [SerializeField] EntrancePortal entrancePortal;


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
        isGamePlaying = false;
        UnderWorldPlayer.Instance.InitPlayer();
        
        StartCoroutine(EnterSequence());
    }

    IEnumerator EnterSequence()
    {
        DirectingManager.Instance.ZoomIn(UnderWorldPlayer.Instance.t_player);
        
        Sequence generatePortalSeq = entrancePortal.GetSeq_GeneratePortal(1.5f);
        Sequence playerEnterPortalSeq = UnderWorldPlayer.Instance.GetSequence_EnterPortal(false, 1f);

        yield return new WaitForSeconds(1f);    //대기시간

        generatePortalSeq.Play();
        yield return new WaitUntil( ()=> generatePortalSeq.IsActive()==false );

        yield return new WaitForSeconds(0.5f);    //대기시간

        playerEnterPortalSeq.Play();
        yield return new WaitUntil( ()=> playerEnterPortalSeq.IsActive()==false );

        entrancePortal.PlaySeq_DestroyPortal(2f);

        yield return new WaitForSeconds(1f);
        DirectingManager.Instance.ZoomOut();
        //
        yield return new WaitForSeconds(1f);
        isGamePlaying = true;
    }


    public void LeaveUnderWorld()
    {
        isGamePlaying = false;
        SceneLoadManager.Instance.Load_MainScene();
    }
}
