using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;
using UnityEngine.EventSystems;

public class Player5 : MonoBehaviour
{

    public float speed;
    public Transform destination;
    public bool canMove = true;
    public bool canSelect = true;
    public Camera vrCamera;
    public float headingForwardSideAngle;
    public float headingForwardUpAngle;
    public Crossing5 currentCrossing;
    AudioManager5 audioManager4;

    private void Start()
    {
        audioManager4 = FindObjectOfType<AudioManager5>();
    }

    void Update()
    {
        if (isHeadingForward() && (Input.GetKey(KeyCode.Space)  || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) && canMove)
        {
            if (audioManager4.GetComponent<AudioSource>().isPlaying)
            {
                Lose();
            }
            else
            {
                Vector3 direction = (destination.position - transform.position).normalized;
                direction.y = 0;
                transform.Translate(direction * speed * Time.deltaTime, Space.World);
            }

        }

        if (canSelect)
        {
            RaycastHit hitInfo;
            Ray ray = new Ray(vrCamera.transform.position, vrCamera.transform.forward);
            if (Physics.Raycast(ray, out hitInfo))
            {
                var selection = hitInfo.transform; 
                Debug.Log(selection);
                Debug.DrawLine(ray.origin, hitInfo.point);
                //if(!audioSource.isPlaying)
                //   { 
                if (hitInfo.transform.tag == "pannello")
                {
                    audioManager4.PlayIWillSurvive();
                    Debug.Log("IWillSurvive");
                    Debug.Log("pannello");
                }
                else if (hitInfo.transform.tag == "pannello1")
                {
                    audioManager4.PlayHakunaMatata();
                    Debug.Log("HakunaMatata");
                    Debug.Log("pannello1");

                }
                else if (hitInfo.transform.tag == "pannello2")
                {
                    audioManager4.PlayTheLionSleepTOnight();
                    Debug.Log("TheLionSleepTonight");
                    Debug.Log("pannello2");
                }
                       
              //  }
            }
            else
            {
                Debug.Log("exit");
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && canSelect)
        {
            RaycastHit hitInfo;
            Ray ray = new Ray(vrCamera.transform.position, vrCamera.transform.forward);
            if (Physics.Raycast(ray, out hitInfo)) 
            {
                Debug.Log("sonoqua");
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
        GetComponent<Collider>().enabled = false;
        canMove = false;
        Vector3 targetPos = currentCrossing.crossingStart.position;
        targetPos.y = 1.5f;
        Quaternion targetRot = Quaternion.Euler(0, currentCrossing.crossingStart.rotation.eulerAngles.y + 180, 0);
        OculusCameraFade.Instance.DoFade(() =>
        {
            transform.position = targetPos;
            transform.rotation = targetRot;
            canMove = true;
            GetComponent<Collider>().enabled = true;

            // Reset all vehicles
            foreach (var vehicle in FindObjectsOfType<Vehicle>())
            {
                vehicle.Reset();
            }
        });
    }
    public void DetectCollision()
    {
        if (canSelect)
        {
            RaycastHit hitInfo;
            Ray ray = new Ray(vrCamera.transform.position, vrCamera.transform.forward);
            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.DrawLine(ray.origin, hitInfo.point);
            }
            else
            {
                Debug.Log("exit");
            }
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Vehicle"))
        {
            Lose();
        }
    }

    public void setEnabled(bool b)
    {
        this.enabled = b;
    }
}
