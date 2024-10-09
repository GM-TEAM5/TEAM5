using UnityEngine;

public abstract class VehicleSO : ScriptableObject
{
    public float speed = 10f;
    public float turnSpeed = 100f;

    public abstract void Move(Transform transform, float verticalInput);

    public abstract void Turn(Transform transform, float horizontalInput);

    public virtual void Fly(Transform transform, bool isAscending, bool isDescending)
    {
    }
}