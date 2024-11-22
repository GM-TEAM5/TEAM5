using System.Collections;
using TMPro;
using UnityEngine;



public class StageTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text_time;

    float startTime = 0;

    Coroutine runningTimer;

    //=============================================================    

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
        
        if( stageWave.goalType == StageGoalType.Survival)
        {
            gameObject.SetActive(true);

            startTime = ((StageWave_Survival)stageWave).targetTime;
            runningTimer = StartCoroutine(RunTimer_Desc());
            
        }
        else
        {
            gameObject.SetActive(false);   
        }
    }

    void OnStageFinished()
    {
        if(runningTimer != null)
        {
            StopCoroutine(runningTimer);
        }
    }

    //==========================================================================


    /// <summary>
    /// 남은 시간 타이머 
    /// </summary>
    /// <returns></returns>
    IEnumerator RunTimer_Desc()
    {
        var waitForSeconds = new WaitForSeconds(1f);
        while(true)
        {
            float curr = startTime - GamePlayManager.Instance.gamePlayTime;
            SetTimer(curr);
            yield return waitForSeconds;
        }
    }



    void SetTimer(float time)
    {
        int mins = (int)time/60;
        int secs = (int)time%60;

        text_time.SetText($"{mins:00}:{secs:00}");
    }
}
