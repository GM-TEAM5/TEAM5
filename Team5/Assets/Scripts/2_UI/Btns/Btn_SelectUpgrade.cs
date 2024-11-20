using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_SelectUpgrade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEventManager.Instance.onSelectItem.AddListener( Deactivate );
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

}
