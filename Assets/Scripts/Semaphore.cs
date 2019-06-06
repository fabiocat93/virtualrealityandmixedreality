﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semaphore : MonoBehaviour
{

    public float greenTime;
    public float yellowTime;
    public float redTime;

    public GameObject greenLight;
    public GameObject yellowLight;
    public GameObject redLight;

    public bool isOn;

    [Tooltip("The minimum distance you have to be away from the sides of the street to lose on a red light")]
    public float redLightLoseDistance;

    private float elapsedTime;

    private Player player;
    private Crossing crossing;

    private enum SemaphoreStatus
    {
        Green, Yellow, Red
    }
    private SemaphoreStatus status;
    
    void Start()
    {
        status = SemaphoreStatus.Green;
        elapsedTime = 0;
        greenLight.SetActive(true);
        yellowLight.SetActive(false);
        redLight.SetActive(false);

        player = FindObjectOfType<Player>();
        crossing = GetComponentInParent<Crossing>();
    }
    
    void Update()
    {
        float a, b;

        if (!isOn)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        switch (status)
        {
            case SemaphoreStatus.Green:
                if(elapsedTime > greenTime)
                {
                    greenLight.SetActive(false);
                    yellowLight.SetActive(true);
                    redLight.SetActive(false);
                    elapsedTime = 0;
                    status = SemaphoreStatus.Yellow;
                }
                break;
            case SemaphoreStatus.Yellow:
                if (elapsedTime > yellowTime)
                {
                    greenLight.SetActive(false);
                    yellowLight.SetActive(false);
                    redLight.SetActive(true);
                    elapsedTime = 0;
                    status = SemaphoreStatus.Red;
                    
                    a = Vector3.Dot(crossing.crossingEnd.position - crossing.crossingStart.position, player.transform.position - crossing.crossingStart.position);
                    b = Vector3.Dot(crossing.crossingEnd.position - crossing.crossingStart.position, crossing.crossingEnd.position - crossing.crossingStart.position);
                    if (a > redLightLoseDistance && a < b - redLightLoseDistance)
                    {
                        // player position lies between crossingStart and crossingEnd (and the semaphore is turning red)
                        player.Lose();
                        elapsedTime = 0;
                        status = SemaphoreStatus.Green;
                    }
                }
                break;
            case SemaphoreStatus.Red:
                if (elapsedTime > redTime)
                {
                    greenLight.SetActive(true);
                    yellowLight.SetActive(false);
                    redLight.SetActive(false);
                    elapsedTime = 0;
                    status = SemaphoreStatus.Green;
                }
                
                a = Vector3.Dot(crossing.crossingEnd.position - crossing.crossingStart.position, player.transform.position - crossing.crossingStart.position);
                b = Vector3.Dot(crossing.crossingEnd.position - crossing.crossingStart.position, crossing.crossingEnd.position - crossing.crossingStart.position);
                if (a > redLightLoseDistance && a < b - redLightLoseDistance)
                {
                    // player position lies between crossingStart and crossingEnd (and the semaphore is actually red)
                    player.Lose();
                    elapsedTime = 0;
                    status = SemaphoreStatus.Green;
                }
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isEditor)
        {
            if(crossing == null)
            {
                crossing = GetComponentInParent<Crossing>();
            }

            Vector3 start = crossing.crossingStart.position;
            Vector3 end = crossing.crossingEnd.position;

            Color baseGizmoColor = Gizmos.color;

            for (float alpha = 0; alpha <= 1; alpha += 0.01f)
            {
                Vector3 point = alpha * start + (1 - alpha) * end;

                float a = Vector3.Dot(end - start, point - start);
                float b = Vector3.Dot(end - start, end - start);
                if (a > redLightLoseDistance && a < b - redLightLoseDistance)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.green;
                }

                Gizmos.DrawCube(point + Vector3.up * 0.2f, Vector3.one / 20);
            }

            Gizmos.color = baseGizmoColor;
        }
    }
}
