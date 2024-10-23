using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ReinforcementPanel : GamePlayPanel 
{
    public readonly int optionsNum = 3;
    
    [SerializeField] TextMeshProUGUI text_reinforceLevel;
    [SerializeField] Transform t_btnParent;
    [SerializeField] GameObject prefab_optionBtn;


    //===========================================================
    protected override void Init()
    {
        //
    }
    
    /// <summary>
    /// 강화 패널 열기 - level 에 따라 더 좋은 선택지 세팅하도록~
    /// </summary>
    /// <param name="level"></param>
    protected override void OnOpen()
    {
        SetOptions(Player.Instance.reinforcementLevel, optionsNum);     // 선택지 세팅. 
    }

    protected override void OnClose()
    {
        
    }

    //=============================================
    void SetOptions(int level, int num)
    {   
        text_reinforceLevel.SetText($"선택지 레벨 {level}");
        
        for(int i=0;i<t_btnParent.childCount;i++)
        {
            Destroy(t_btnParent.GetChild(i).gameObject);
        }
        //
        for(int i=0;i<num;i++)
        {
            ReinforcementOptionBtn optionBtn = Instantiate(prefab_optionBtn.gameObject, t_btnParent).GetComponent<ReinforcementOptionBtn>();
            optionBtn.SetOption( new TestOptionData(null, $"이름 {i}", $"내용 {i}"));    // 나중에는 옵션 랜덤으로 뽑아다가 쓸거임. 
        }
    }


}
