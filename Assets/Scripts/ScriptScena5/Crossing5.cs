using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossing5 : MonoBehaviour
{

    public Transform crossingStart;
    public Transform crossingEnd;
    // la posizione iniziale all'incrocio
    public Transform nextWaypoint;
    public float minDistance;
    public GameController5 gameController;

    private Player5 player;
    private bool isTeleporting = false;

    private void Start()
    {
        player = FindObjectOfType<Player5>();
    }

    void Update()
    {
        // controlla se hai vinto

        Vector3 direction = player.transform.position - crossingEnd.position;
        direction.y = 0;
        if (direction.sqrMagnitude < minDistance * minDistance)
        {
            gameController.teleportToTheNextScene();
            enabled = false;
            Debug.Log("Il giocatore ha superato il livello");
/*
            player.canMove = false;
            player.canSelect = true;

            foreach (var camera in GetComponentsInChildren<Camera>())
            {
                camera.enabled = false;
            }

            if (!isTeleporting)
            {
                isTeleporting = true;
                Invoke("Teleport", 3);
            }
*/
        }
    }
/*
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
*/
}
