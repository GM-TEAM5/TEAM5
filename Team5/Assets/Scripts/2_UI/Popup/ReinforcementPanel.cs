using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ReinforcementPanel : MonoBehaviour
{
    public readonly int optionsNum = 3;
    [SerializeField] Transform t_btnParent;
    [SerializeField] GameObject prefab_optionBtn;


    public void Open()
    {
        SetOptions(optionsNum);     // 선택지 세팅. 
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }


    //=============================================
    void SetOptions(int num)
    {   
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
