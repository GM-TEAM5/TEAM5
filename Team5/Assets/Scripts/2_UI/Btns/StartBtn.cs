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
        // 저장된 게임 정보를 읽고 거기서 부터 시작할 수 있게
        // 기획에 따라 - 대기 마을로 진행, 컷씬 스킵 등을 구현할 예정.
        SceneLoadManager.Instance.LoadScene(SceneLoadManager.cutSceneName);
    }



}
