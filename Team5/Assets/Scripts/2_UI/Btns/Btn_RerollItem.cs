using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class Btn_RerollItem : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] TextMeshProUGUI text_btn;
    
    void Start()
    {
        Init();
    }
    
    public void Init()
    {
        GameEventManager.Instance.onReroll.AddListener((a) => UpdateBtn() );


        UpdateBtn();
    }

    void UpdateBtn()
    {
        text_btn.SetText($"재입고 ({Player.Instance.status.rerollCount})");
    }




}
