using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform t;
    Transform t_camera;


    void Start()
    {
        t = transform;
        t_camera = Camera.main.transform;
    }

    void Update()
    {      
        // Debug.Log(t.parent.name + "  " +dir);
        t.rotation = Quaternion.LookRotation(t.position - t_camera.position);
        t.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,0,0);
    }
}
