using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public VehicleSO vehicleData;
    private bool isOccupied = false;
    // private PlayerController occupant;

    // public void EnterVehicle(PlayerController player)
    // {
    //     occupant = player;
    //     player.transform.SetParent(transform);
    //     player.transform.localPosition = Vector3.zero;
    //     isOccupied = true;
    // }

    // public void ExitVehicle(PlayerController player)
    // {
    //     occupant.transform.SetParent(null);
    //     occupant = null;
    //     isOccupied = false;
    // }

    public void ControlVehicle(float horizontal, float vertical)
    {
        if (!isOccupied)
        {
            return;
        }

        // 차량 이동 및 회전
        vehicleData.Move(transform, vertical);
        vehicleData.Turn(transform, horizontal);

        // 비행 물체
        vehicleData.Fly(transform, Input.GetKey(KeyCode.Space), Input.GetKey(KeyCode.LeftShift));
    }
}