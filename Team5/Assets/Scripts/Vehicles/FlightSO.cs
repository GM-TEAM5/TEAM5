using UnityEngine;

[CreateAssetMenu(fileName = "Flight", menuName = "Vehicles/Flight")]
public class FlightSO : VehicleSO
{
    [Header("Flight Settings")] 
    public float liftSpeed = 5f;
    public float maxAltitude = 100f;
    public float tiltAngle = 15f;

    public override void Move(Transform transform, float verticalInput)
    {
        Vector3 moveDirection = transform.forward * verticalInput * speed * Time.fixedDeltaTime;
        transform.position += moveDirection;

        // 이동시 기울기 조절
        float currentTiltAngle =
            Mathf.LerpAngle(transform.rotation.eulerAngles.x, verticalInput * tiltAngle, 2f * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(currentTiltAngle, transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z);
    }

    public override void Turn(Transform transform, float horizontalInput)
    {
        transform.Rotate(Vector3.up, horizontalInput * turnSpeed * Time.fixedDeltaTime);
    }

    public override void Fly(Transform transform, bool isAscending, bool isDescending)
    {
        Vector3 moveDirection = Vector3.zero;

        if (isAscending)
        {
            moveDirection = transform.up * 1 * liftSpeed * Time.deltaTime;
        }
        else if (isDescending)
        {
            moveDirection = transform.up * -1 * liftSpeed * Time.deltaTime;
        }

        if (transform.position.y > maxAltitude)
        {
            moveDirection.y = 0;
        }

        transform.position += moveDirection;
    }
}