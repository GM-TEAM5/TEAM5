using UnityEngine;

[CreateAssetMenu(fileName = "Car", menuName = "Vehicles/Car")]
public class CarSO : VehicleSO
{
    public override void Move(Transform transform, float verticalInput)
    {
        Vector3 moveDirection = transform.forward * verticalInput * speed * Time.deltaTime;
        transform.position += moveDirection;
    }

    public override void Turn(Transform transform, float horizontalInput)
    {
        transform.Rotate(Vector3.up, horizontalInput * turnSpeed * Time.deltaTime);
    }
}