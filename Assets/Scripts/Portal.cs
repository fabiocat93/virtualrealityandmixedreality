using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    public Crossing destination;
    public bool isRight;

    private Hub hub;

    private void Start()
    {
        hub = GetComponentInParent<Hub>();
    }

    public void EnterPortal()
    {
        if (!isRight)
        {
            hub.WrongChoiceMade();
            return;
        }
        
        Player player = FindObjectOfType<Player>();
        player.canMove = true;
        player.canSelect = false;
        Vector3 targetPos = destination.crossingStart.position;
        targetPos.y = 1.5f;
        Quaternion targetRot = Quaternion.Euler(0, destination.crossingStart.rotation.eulerAngles.y + 180, 0);
        OculusCameraFade.Instance.DoFade(() =>
        {
            player.transform.position = targetPos;
            player.transform.rotation = targetRot;

            player.currentCrossing = destination;

            Semaphore semaphore = destination.GetComponentInChildren<Semaphore>();
            if(semaphore != null)
            {
                semaphore.isOn = true;
            }

            if (!destination.stopAllVehicles)
            {
                // Start all vehicles
                foreach (var vehicle in FindObjectsOfType<Vehicle>())
                {
                    vehicle.Stop = false;
                }

                // Start all spawners
                foreach (var spawner in FindObjectsOfType<VehicleSpawner>())
                {
                    spawner.canSpawn = true;
                }
            }
        });
    }
}
