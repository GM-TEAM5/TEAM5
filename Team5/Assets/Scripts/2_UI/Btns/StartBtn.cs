using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StartBtn : MonoBehaviour
{

    void Start()
    {
        GetComponent<Button>().onClick.AddListener( StartGame );

    }

    void StartGame()
    {
        // 신규유저의 경우 컷씬 재생
        if( GameManager.Instance.playerData.isNewUser)
        {
            SceneLoadManager.Instance.LoadScene(SceneLoadManager.cutSceneName);
        }
        // 이미 컷씬을 본경우, 대기실로 이동.
        else
        {
            SceneLoadManager.Instance.Load_UnderWorld();
        }
    }



}
