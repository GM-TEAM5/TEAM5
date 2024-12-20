using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerSkillsUI : MonoBehaviour
{
    [SerializeField] GameObject prefab_skillUI;
    [SerializeField] List<PlayerSkillUI> skillUIs;


    void Awake()
    {
        GameEventManager.Instance.onInitPlayer.AddListener( ( )=>Init(Player.Instance._skills) );
    }



    public void Init(SerializableDictionary<KeyCode,PlayerSkill> skills)
    {
        // 미리 만들어져 있던 거 파괴
        for(int i=0;i<transform.childCount;i++)
        {
            Destroy( transform.GetChild(i).gameObject );
        }

        //  
        skillUIs = new();

        // 생성 
        foreach( var kv in skills)
        {
            KeyCode keyCode = kv.Key;
            PlayerSkill playerSkill = kv.Value;

            PlayerSkillUI skillUI = Instantiate(prefab_skillUI, transform).GetComponent<PlayerSkillUI>();
            skillUI.Init( keyCode, playerSkill );

            skillUIs.Add(skillUI);
        }
    }
}
