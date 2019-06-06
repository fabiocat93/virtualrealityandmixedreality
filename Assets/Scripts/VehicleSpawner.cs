using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{

    public Vehicle[] randomlySpawnableVehicles;
    public SpawnerEvent[] events;
    public Transform spawnPosition;

    private int nextEventIndex = 0;
    private float time;

    [HideInInspector]
    public bool canSpawn = false;
    
    void Update()
    {
        if (!canSpawn)
        {
            return;
        }

        time += Time.deltaTime;

        if(time > events[nextEventIndex].delayFromPrevious)
        {
            HandleNextEvent();
            time = 0;
        }
    }

    private void HandleNextEvent()
    {
        SpawnerEvent e = events[nextEventIndex];
        Vehicle prefab = e.vehicle == null ? randomlySpawnableVehicles[Random.Range(0, randomlySpawnableVehicles.Length)] : e.vehicle;
        Vehicle vehicle = Instantiate(prefab, spawnPosition.position, spawnPosition.rotation);
        vehicle.wasSpawned = true;
        vehicle.Stop = false;
        if(e.speed > 0)
        {
            vehicle.speed = e.speed;
        }
        nextEventIndex = (nextEventIndex + 1) % events.Length;
    }

    [System.Serializable]
    public struct SpawnerEvent
    {
        public float delayFromPrevious;
        [Tooltip("If not filled, a random vehicle will be spawned")]
        public Vehicle vehicle;
        [Tooltip("If < 0, the base speed of the spawned vehicle will be used")]
        public float speed;
    }
}
