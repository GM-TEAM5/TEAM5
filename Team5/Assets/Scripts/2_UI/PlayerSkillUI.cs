using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillUI : MonoBehaviour
{
    KeyCode keyCode;
    
    PlayerSkill playerSkill;
    
    [SerializeField] Image img_icon;
    [SerializeField] TextMeshProUGUI text_keyCode;
    
    void Awake()
    {
        GameEventManager.Instance.onChangeSkill.AddListener(OnChangeSkill);
    }


    public void Init(KeyCode keyCode, PlayerSkill playerSkill)
    {        
        this.keyCode = keyCode;
        this.playerSkill = playerSkill;


        text_keyCode.SetText(keyCode.ToString());
        

        // 그리고 쿨타임도 초기화.
        if(playerSkill.skillData !=null)
        {
            img_icon.sprite = playerSkill.skillData?.sprite;
            Activate();
            
        }
        else
        {
            Deactivate();
        }
        
    }


    /// <summary>
    /// 스킬 바뀔 때 호출됨. 바뀐 스킬칸의 경우에만 반응하도록함. 
    /// </summary>
    /// <param name="keyCode"></param>
    /// <param name="playerSkill"></param>
    public void OnChangeSkill(KeyCode keyCode, PlayerSkill playerSkill)
    {        
        if (this.keyCode != keyCode)
        {
            return;
        }
        
        this.playerSkill = playerSkill;

        text_keyCode.SetText(keyCode.ToString());

        // 그리고 쿨타임도 초기화.
        if(playerSkill.skillData !=null)
        {
            img_icon.sprite = playerSkill.skillData?.sprite;
            Activate();
            
        }
        else
        {
            Deactivate();
        }
    }



    public void Activate()
    {
        img_icon.gameObject.SetActive(true);    
    }

    public void Deactivate()
    {
        img_icon.gameObject.SetActive(false);
    }
}
