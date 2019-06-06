using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossing : MonoBehaviour
{

    public Transform crossingStart;
    public Transform crossingEnd;
    public Transform nextWaypoint;
    public float minDistance;
    public Crossing nextCrossing;

    private Player player;
    private bool isTeleporting = false;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        Vector3 direction = player.transform.position - crossingEnd.position;
        direction.y = 0;
        if (direction.sqrMagnitude < minDistance * minDistance)
        {
            if(nextCrossing == null)
            {
                // THE END
                player.canMove = false;
                return;
            }

            player.canMove = false;
            player.canSelect = true;

            foreach (var camera in GetComponentsInChildren<Camera>())
            {
                camera.enabled = false;
            }
            foreach (var camera in nextCrossing.GetComponentsInChildren<Camera>())
            {
                camera.enabled = true;
            }

            foreach (var portal in nextWaypoint.transform.parent.GetComponentsInChildren<Portal>())
            {
                portal.destination = nextCrossing;
            }

            player.destination = nextCrossing.crossingEnd;

            if (!isTeleporting)
            {
                isTeleporting = true;
                Invoke("Teleport", 3);
            }
        }
    }

    void Teleport()
    {
        Vector3 targetPos = nextWaypoint.position;
        Quaternion targetRot = Quaternion.Euler(0, 0, 0);
        OculusCameraFade.Instance.DoFade(() => {
            player.transform.position = targetPos;
            player.transform.rotation = targetRot;

            Semaphore semaphore = GetComponentInChildren<Semaphore>();
            if (semaphore != null)
            {
                semaphore.isOn = true;
            }
            isTeleporting = false;
        });
    }
}
