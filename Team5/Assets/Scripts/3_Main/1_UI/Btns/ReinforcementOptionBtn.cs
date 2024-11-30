using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 테스트용 선택지 데이터, - 나중에 SO로 바꿀거임. 
[Serializable]
public class TestOptionData 
{
    public Sprite icon;
    public string name;
    public string desc; 

    public TestOptionData(Sprite icon, string name, string desc)
    {
        this.icon = icon;
        this.name = name;
        this.desc = desc;
    }

}

/// <summary>
/// 레벨업 시 강화 선택지 버튼
/// </summary>
public class ReinforcementOptionBtn : MonoBehaviour
{
    [SerializeField] TestOptionData optionData;
    
    Button btn;
    
    [SerializeField] Image img_icon;     // 
    [SerializeField] TextMeshProUGUI text_name;  
    [SerializeField] TextMeshProUGUI text_desc;  

    //====================================================


    /// <summary>
    /// 레벨업 선택지 버튼을 눌렀을 때, 
    /// </summary>
    void OnClick()
    {
        // 데이터에 맞춰 능력 및 수치 변경

        //
        GamePlayManager.Instance.OnSelect_ReinforcementOption();

        Player.Instance.reinforcementLevel ++;
    }


    //==============================
    /// <summary>
    /// option data를 받아 아이콘과 설명, 그리고 선택시 이벤트를 설정한다. 
    /// </summary>
    public void SetOption(TestOptionData optionData) 
    {
        this.optionData = optionData;
        
        btn = GetComponent<Button>();
        btn.onClick.AddListener( OnClick );
        
        SetLayout();
        
        img_icon.sprite = optionData.icon;
        text_name.SetText( optionData.name );
        text_desc.SetText( optionData.desc );
    }

    void SetLayout()
    {
        
    }

}



