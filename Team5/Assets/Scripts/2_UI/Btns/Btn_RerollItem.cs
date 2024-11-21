using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class Btn_RerollItem : MonoBehaviour
{
    // [SerializeField] Button btn;
    // [SerializeField] TextMeshProUGUI text_btn;
    
    void Start()
    {
        Init();
    }
    
    public void Init()
    {
        GameEventManager.Instance.onUpgradeReroll.AddListener(OnReroll);
        GameEventManager.Instance.onSelectItem.AddListener( Deactivate );


        OnReroll();
    }

    void OnReroll()
    {
        gameObject.SetActive(Player.Instance.status.rerollCount > 0); 
    }


    void Deactivate()
    {
        gameObject.SetActive(false);
    }




}
