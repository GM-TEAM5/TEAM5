using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerStatusUpgradeProgressUI : MonoBehaviour
{
    public PlayerStatusUpgradeProgressData data;

    Toggle toggle;


    [Header("Field")]
    [SerializeField] TextMeshProUGUI text_fieldName;    // 선택되었으면, 글자 색깔을 변경한다. 
    [SerializeField] Color color_default = new Color(0.4f,0.4f,0.4f);
    [SerializeField] Color color_selected = Color.black;
    
    
    [Header("ProgressBar")]
    [SerializeField] GameObject prefab_progressLevel;   // 진행도 한칸
    [SerializeField] Transform t_progress;
    [SerializeField] List<ProgressLevelUI> progressLevels;       // 현재 레벨에 맞게, 그리고 선택되었으면 진행도를 변경한다. 

    [Header("Info")]
    [SerializeField] PlayerStatusUpgradePanel parent;   // 토글 시 정보를 디스플레이 할 오브젝트 .



    //========================================================================================

    public void Init(PlayerStatusUpgradeProgressData data,ToggleGroup toggleGroup, PlayerStatusUpgradePanel parent)
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggle);
        toggle.group = toggleGroup;
        
        this.data = data;

        this.parent = parent;

        InitFieldName();
        InitProgress();
    }

    void InitFieldName()
    {
        text_fieldName.SetText(data.displayedName);
    }

    void InitProgress()
    {
        //
        for(int i=0;i<t_progress.childCount;i++)
        {
            Destroy(t_progress.GetChild(i).gameObject);
        }

        progressLevels = new();
        for(int i=0;i<data.maxLevel;i++)
        {
            ProgressLevelUI progressLevelUI = Instantiate(prefab_progressLevel, t_progress).GetComponent<ProgressLevelUI>();
            progressLevelUI.Init();
            progressLevels.Add( progressLevelUI );

        }
    }

    public void OnOpen()
    {
        SyncProgress();
    }


    void SyncProgress()
    {
        int currLevel = data.level;
        for(int i=0;i<progressLevels.Count;i++)
        {
            if (i< currLevel)
            {
                progressLevels[i].SetState( PlayerStatusUpgradeProgressState.Completed );
            }
            else if ( toggle.isOn && i == currLevel)
            {
                progressLevels[i].SetState( PlayerStatusUpgradeProgressState.Selected);
            }
            else
            {
                progressLevels[i].SetState( PlayerStatusUpgradeProgressState.Unvalid );
            }
            
        }
    }

    void OnToggle(bool isOn)
    {
        SyncProgress();
        // Debug.Log($"{gameObject.name} : {isOn}");

        if(isOn)
        {
            text_fieldName.color = color_selected;
            parent.DisplayData(this);   // 정보 띄우기
        }
        else
        {
            text_fieldName.color = color_default;
        }
    }




}
