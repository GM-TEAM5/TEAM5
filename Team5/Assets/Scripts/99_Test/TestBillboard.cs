using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBillboard : MonoBehaviour
{
    Transform t_camera;
    Transform t_sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        t_camera = Camera.main.transform;
        t_sprite = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Billboard();
    }

        void Billboard()
    {
        t_sprite.rotation = Quaternion.LookRotation(t_sprite.position - t_camera.position);
        t_sprite.rotation = Quaternion.Euler(t_sprite.rotation.eulerAngles.x,0,0);
    }
}
