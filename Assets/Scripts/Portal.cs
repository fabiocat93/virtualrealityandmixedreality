using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    public Transform destination;

    public void EnterPortal()
    {
        Player player = FindObjectOfType<Player>();
        player.canMove = true;
        player.canSelect = false;
        Vector3 targetPos = destination.position;
        targetPos.y = 1.5f;
        Quaternion targetRot = Quaternion.Euler(0, destination.rotation.eulerAngles.y + 180, 0);
        OculusCameraFade.Instance.DoFade(() =>
        {
            player.transform.position = targetPos;
            player.transform.rotation = targetRot;
        });
    }
}
