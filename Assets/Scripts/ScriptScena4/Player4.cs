using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;
using UnityEngine.EventSystems;

public class Player4 : MonoBehaviour
{

    public float speed;
    public Transform destination;
    public bool canMove = true;
    public bool canSelect = false;
    public Camera vrCamera;
    public float headingForwardSideAngle;
    public float headingForwardUpAngle;
    public Crossing4 currentCrossing;
    AudioManager4 audioManager4;

    public GameController4 gameController;

    private void Start()
    {
        audioManager4 = FindObjectOfType<AudioManager4>();
    }


    void Update()
    {
        if (isHeadingForward() && (Input.GetKey(KeyCode.Space) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) && canMove)
        {
            if (audioManager4.GetComponent<AudioSource>().isPlaying)
            {
                gameController.LoseWithMusic();
            }
            else
            {
                Vector3 direction = (destination.position - transform.position).normalized;
                direction.y = 0;
                transform.Translate(direction * speed * Time.deltaTime, Space.World);
            }
        }

        Debug.Log(canSelect);
       /* if (!audioManager4.GetComponent<AudioSource>().isPlaying)
        {
            canSelect = true;
        }
        */
        if (canSelect && Application.isEditor)
        {
            Debug.Log("enter in canselect");
            RaycastHit hitInfo;
            Ray ray = new Ray(vrCamera.transform.position, vrCamera.transform.forward);
            if (Physics.Raycast(ray, out hitInfo))
            {
                /*var selection = hitInfo.transform;
                Debug.Log(selection);
                Debug.DrawLine(ray.origin, hitInfo.point);
               
                
                if (hitInfo.transform.tag == "pannello" && !audioManager4.IsPlayingIWillSurvive())
                {
                    audioManager4.PlayIWillSurvive();
                    //canSelect = false;
                    Debug.Log("IWillSurvive");
                    Debug.Log("pannello");
                }
                else if (hitInfo.transform.tag == "pannello1" && !audioManager4.IsPlayingHakunaMatata())
                {
                    audioManager4.PlayHakunaMatata();
                    //canSelect = false;
                        
                    Debug.Log("HakunaMatata");
                    Debug.Log("pannello1");

                }
                else if (hitInfo.transform.tag == "pannello2" && !audioManager4.IsPlayingTheLionSleepTonight())
                {
                    audioManager4.PlayTheLionSleepTOnight();
                    //canSelect = false;
                    Debug.Log("TheLionSleepTonight");
                    Debug.Log("pannello2");
                }*/

                EventTrigger eventTrigger = hitInfo.collider.gameObject.GetComponent<EventTrigger>();
                if (eventTrigger != null)
                {
                    eventTrigger.OnPointerEnter(new PointerEventData(EventSystem.current));
                }

                //if(!audioSource.isPlaying)
                //   { 


                //  }
            }
            else
            {
                Debug.Log("exit");
            }
        }


        if (Input.GetKeyUp(KeyCode.Space) && canSelect && Application.isEditor)
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
        gameController = FindObjectOfType<GameController4>();

        if (other.gameObject.layer == LayerMask.NameToLayer("Vehicle"))
        {
            gameController.LoseWithIncident();
        }
    }

    public void setEnabled(bool b)
    {
        this.enabled = b;
    }
}
