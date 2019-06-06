using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossingEnd : MonoBehaviour
{

    public Transform nextWaypoint;
    public float minDistance;
    public Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        if (direction.sqrMagnitude < minDistance * minDistance)
        {
            player.canMove = false;
            player.canSelect = true;
            Invoke("Teleport", 3);
        }
    }

    void Teleport()
    {
        Vector3 targetPos = nextWaypoint.position;
        Quaternion targetRot = Quaternion.Euler(0, 0, 0);
        OculusCameraFade.Instance.DoFade(() => {
            player.transform.position = targetPos;
            player.transform.rotation = targetRot;
        });
    }
}
