using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle2 : MonoBehaviour
{

    public float destroyAfterSeconds;
    [Tooltip("Probability that at an intersection the vehicle will continue forward"), Range(0, 1)]
    public float forwardProbability;

    public float speed;
    
    private bool stop = true;
    public bool Stop { set { stop = value; } }
    private Player2 player;
    private TrafficGridPoint gridPoint;

    private Vector3 startingPosition;
    private Quaternion startingRotation;
    [HideInInspector]
    public bool wasSpawned = false;

    private void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;

        player = FindObjectOfType<Player2>();
        FindNearestGridPoint();
        FaceNextGridPoint();
    }

    void Update()
    {
        destroyAfterSeconds -= Time.deltaTime;

        if(wasSpawned && destroyAfterSeconds < 0 && !(CanPlayerSeeMe()))
        {
            Destroy(gameObject);
        }

        if (stop)
        {
            return;
        }

        Vector3 targetPos = gridPoint.transform.position;
        targetPos.y = transform.position.y;
        if ((transform.position - targetPos).sqrMagnitude < 0.1f)
        {
            if (gridPoint.HasNext())
            {
                if (Random.Range(0.0f, 1.0f) < forwardProbability)
                {
                    gridPoint = gridPoint.ForwardNext(transform.forward);
                }
                else
                {
                    gridPoint = gridPoint.RandomNext();
                }

                FaceNextGridPoint();
            }
            else
            {
                // Stop
                stop = true;
            }
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    private void FindNearestGridPoint()
    {
        TrafficGridPoint[] allTrafficGridPoints = FindObjectsOfType<TrafficGridPoint>();
        float minDist = Mathf.Infinity;

        foreach (var point in allTrafficGridPoints)
        {
            if (point.nextPoints.Length == 0) continue;
            Vector3 dir = point.transform.position - transform.position;
            if (Vector3.Dot(dir, transform.forward) > 0 && dir.sqrMagnitude < minDist)
            {
                minDist = dir.sqrMagnitude;
                gridPoint = point;
            }
        }
    }

    private void FaceNextGridPoint()
    {
        Vector3 targetPos = gridPoint.transform.position;
        targetPos.y = transform.position.y;
        Vector3 dir = (targetPos - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    private void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = Color.green;

        if (gridPoint == null)
        {
            FindNearestGridPoint();
        }

        Gizmos.DrawLine(transform.position + Vector3.up * 1.2f, gridPoint.transform.position + Vector3.up * 1.2f);

        Gizmos.color = oldColor;
    }

    public bool CanPlayerSeeMe()
    {
        RaycastHit hitInfo;
        Vector3 headingDir = player.vrCamera.transform.forward;
        if(Physics.Raycast(player.transform.position, headingDir, out hitInfo))
        {
            return hitInfo.collider.gameObject == this;
        }
        return false;
    }

    public void Reset()
    {
        if (wasSpawned)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = startingPosition;
            transform.rotation = startingRotation;
            Start();
        }
    }
}
