using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficGridPoint : MonoBehaviour
{

    public TrafficGridPoint[] nextPoints;

    private void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = Color.blue;

        if(nextPoints.Length == 0)
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawSphere(transform.position + Vector3.up, 1f);
        Gizmos.color = Color.blue;

        foreach (var point in nextPoints)
        {
            Gizmos.DrawLine(transform.position + Vector3.up, point.transform.position + Vector3.up);
        }

        Gizmos.color = oldColor;
    }

    public TrafficGridPoint RandomNext()
    {
        return nextPoints[Random.Range(0, nextPoints.Length)];
    }

    public bool HasNext()
    {
        return nextPoints.Length > 0;
    }

    public TrafficGridPoint ForwardNext(Vector3 forward)
    {
        TrafficGridPoint best = null;
        float maxDot = -Mathf.Infinity;

        foreach (var point in nextPoints)
        {
            float dot = Vector3.Dot((point.transform.position - transform.position).normalized, forward);
            if(dot > maxDot)
            {
                maxDot = dot;
                best = point;
            }
        }

        return best == null ? RandomNext() : best;
    }
}
