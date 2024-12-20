using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Btn_Neng : MonoBehaviour
{
    [SerializeField] Button btn;

    void Start()
    {
        btn = GetComponent<Button>();

        btn.onClick.AddListener( SceneLoadManager.Instance.Load_Lobby);
    }
}
