using System.Collections;
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

    void Update()
    {
        if ((Input.GetKey(KeyCode.Space) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) && canMove)
        {
            Vector3 direction = (destination.position - transform.position).normalized;
            direction.y = 0;
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }

        if ((Input.GetKey(KeyCode.Space) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) && canSelect)
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

    private void OnDrawGizmos()
    {
        if (canSelect)
        {
            Ray ray = new Ray(vrCamera.transform.position, vrCamera.transform.forward);
            Gizmos.DrawLine(ray.GetPoint(0), ray.GetPoint(100));
        }
    }
}
