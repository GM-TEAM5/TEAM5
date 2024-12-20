using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillUI : MonoBehaviour
{
    SkillType skillType;

    PlayerSkill playerSkill;

    [SerializeField] Image img_icon;
    [SerializeField] TextMeshProUGUI text_keyCode;

    void Awake()
    {
        GameEventManager.Instance.onChangeSkill.AddListener(OnChangeSkill);
    }


    public void Init(SkillType skillType, PlayerSkill playerSkill)
    {
        this.skillType = skillType;
        this.playerSkill = playerSkill;

        text_keyCode.SetText(skillType.ToString());

        if (playerSkill?.skillData != null)
        {
            img_icon.sprite = playerSkill.skillData.sprite;
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
        if (this.skillType != playerSkill.skillData.skillType)
        {
            return;
        }

        this.playerSkill = playerSkill;

        text_keyCode.SetText(skillType.ToString());

        if (playerSkill.skillData != null)
        {
            img_icon.sprite = playerSkill.skillData.sprite;
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
