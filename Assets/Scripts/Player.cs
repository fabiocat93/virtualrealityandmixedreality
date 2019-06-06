﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{

    public float speed;
    public Transform destination;
    public bool canMove = true;
    public bool canSelect = true;
    public Camera vrCamera;
    public float headingForwardSideAngle;
    public float headingForwardUpAngle;
    public Crossing currentCrossing;

    void Update()
    {
        if (isHeadingForward() && (Input.GetKey(KeyCode.Space) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) && canMove)
        {
            Vector3 direction = (destination.position - transform.position).normalized;
            direction.y = 0;
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }

        if (Input.GetKeyUp(KeyCode.Space) && canSelect)
        {
            RaycastHit hitInfo;
            Ray ray = new Ray(vrCamera.transform.position, vrCamera.transform.forward);
            if (Physics.Raycast(ray, out hitInfo)) {
                EventTrigger eventTrigger = hitInfo.collider.gameObject.GetComponent<EventTrigger>();
                if(eventTrigger != null)
                {
                    eventTrigger.OnPointerUp(new PointerEventData(EventSystem.current));
                }
            }
        }
    }

    public void Lose()
    {
        Debug.LogError("Player has lost");
        canMove = false;
        Vector3 targetPos = currentCrossing.crossingStart.position;
        targetPos.y = 1.5f;
        Quaternion targetRot = Quaternion.Euler(0, currentCrossing.crossingStart.rotation.eulerAngles.y + 180, 0);
        OculusCameraFade.Instance.DoFade(() =>
        {
            transform.position = targetPos;
            transform.rotation = targetRot;
            canMove = true;
        });
    }

    private bool isHeadingForward()
    {
        Vector3 headingDir = vrCamera.transform.forward;
        headingDir.y = 0;
        Vector3 forwardDir = (destination.position - transform.position).normalized;
        forwardDir.y = 0;
        float sideAngle = Vector3.Angle(forwardDir, headingDir);
        float upAngle = Vector3.Angle(vrCamera.transform.forward, headingDir);

        return Mathf.Abs(sideAngle) < headingForwardSideAngle && Mathf.Abs(upAngle) < headingForwardUpAngle;
    }

    private void OnDrawGizmos()
    {
        if (canSelect)
        {
            Ray ray = new Ray(vrCamera.transform.position, vrCamera.transform.forward);
            Gizmos.DrawLine(ray.GetPoint(0), ray.GetPoint(100));
        }
    }
}
