using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyMouseCursor : MonoBehaviour
{
    void LateUpdate()
    {
        transform.position = PlayerInputManager.Instance.mousePosition;
    }
}
