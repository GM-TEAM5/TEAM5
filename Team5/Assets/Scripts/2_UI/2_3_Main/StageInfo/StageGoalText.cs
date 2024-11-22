using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageGoalText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    void Awake()
    {
        Init();
    }

     public void Init()
    {
        gameObject.SetActive(false);

        GameEventManager.Instance.onStageStart.AddListener( InitByStageInfo );
        GameEventManager.Instance.onStageFinish.AddListener(OnStageFinished);
    }

     public void InitByStageInfo(StageWave stageWave)
    {
        // Debug.Log("기머띠");
        gameObject.SetActive(true);
        if( stageWave.goalType == StageGoalType.Survival)
        {
            text.SetText("제한 시간 동안 생존하세요");
            
        }
        else
        {
            text.SetText("모든 적을 섬멸하세요");
        }
    }

    void OnStageFinished()
    {
        text.SetText("스테이지 클리어");
    }

}
