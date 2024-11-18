using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingArea : MonoBehaviour
{
    [SerializeField] float targetRadius;
    [SerializeField] float activationTime;
    [SerializeField] float deactivationTime;
    [SerializeField] bool isActivated;

    public void Init()
    {
        transform.localScale = targetRadius * Vector3.one;
        gameObject.SetActive(false);
    }


    public void Activate()
    {
        gameObject.SetActive(true);

    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
