using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController3 : MonoBehaviour
{

    public Hub3 hub;
    public Crossing3 crossing;
    public AudioManager3 audioManager;

    private Player3 player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player3>();
        player.setEnabled(false);
        float lengthAudio = audioManager.PlayStartHub();
        StartCoroutine(WaitAudioToBeFinishedAndEnablePlayer(lengthAudio));
    }

    public IEnumerator WaitAudioToBeFinishedAndEnablePlayer(float lengthAudio)
    {
        yield return new WaitForSeconds(lengthAudio);
        Debug.Log("WaitAndStartChoiceInHub");
        player.setEnabled(true);
    }

    public IEnumerator WaitAudioToBeFinishedAndEnablePlayerAndCars(float lengthAudio)
    {
        yield return new WaitForSeconds(lengthAudio);
        Debug.Log("WaitAudioToBeFinishedAndEnablePlayerAndCars");
        player.setEnabled(true);

        // Start all vehicles
        foreach (var vehicle in FindObjectsOfType<Vehicle3>())
        {
            Debug.Log("Veicolo trovato: " + vehicle);
            vehicle.GetComponent<AudioSource>().mute = false;
            vehicle.Stop = false;
        }

        // Start all spawners
        foreach (var spawner in FindObjectsOfType<VehicleSpawner3>())
        {
            Debug.Log("spawner trovato: " + spawner);

            spawner.canSpawn = true;
        }
    }


    public IEnumerator WaitAudioToBeFinishedAndTeleportToCrossing(float lengthAudio)
    {
        Semaphore3 semaphore = crossing.GetComponentInChildren<Semaphore3>();
        semaphore.enabled = false;
        yield return new WaitForSeconds(lengthAudio);
        Debug.Log("WaitAudioToBeFinishedAndTeleportToCrossing");

        player.setEnabled(true);

        player.canMove = true;
        player.canSelect = false;
        Vector3 targetPos = crossing.crossingStart.position;
        targetPos.y = 1.5f;
        Quaternion targetRot = Quaternion.Euler(0, crossing.crossingStart.rotation.eulerAngles.y + 180, 0);
        OculusCameraFade.Instance.DoFade(() =>
        {
            player.transform.position = targetPos;
            player.transform.rotation = targetRot;

            player.currentCrossing = crossing;

           // Semaphore3 semaphore = crossing.GetComponentInChildren<Semaphore3>();
            if (semaphore != null)
            {
                semaphore.isOn = true;
            }

            player.setEnabled(false);
            float lengthAudio2 = audioManager.PlayStartCrossing();
            StartCoroutine(WaitAudioToBeFinishedAndEnablePlayerAndCars(lengthAudio2));
        });
    }

    public void LoseWithIncident()
    {
        player.setEnabled(false);
        player.Lose();

        // Start all vehicles
        foreach (var vehicle in FindObjectsOfType<Vehicle3>())
        {
            Debug.Log("Veicolo trovato: " + vehicle);
            vehicle.Reset();
        }


        float lengthAudio = audioManager.PlayWrongIncident();
        StartCoroutine(WaitAudioToBeFinishedAndEnablePlayerAndCars(lengthAudio));

    }

    public IEnumerator WaitAudioToBeFinishedAndRestartLevel(float lengthAudio)
    {
        yield return new WaitForSeconds(lengthAudio);
        Debug.Log("WaitAudioToBeFinishedAndRestartLevel");
        player.Lose();

        // Start all vehicles
        foreach (var vehicle in FindObjectsOfType<Vehicle3>())
        {
            Debug.Log("Veicolo trovato: " + vehicle);
            vehicle.Reset();
        }

        player.setEnabled(true);
        Semaphore3 semaphore = crossing.GetComponentInChildren<Semaphore3>();
        semaphore.enableSemaphore();
    }

    public void makeThePlayerLooseStrips()
    {
        player.setEnabled(false);
        float lengthAudio = audioManager.PlayWrongCrossingStrips();
        StartCoroutine(WaitAudioToBeFinishedAndRestartLevel(lengthAudio));
    }

    public IEnumerator WaitAudioToBeFinishedAndTeleportToNextScene(float lengthAudio)
    {
        yield return new WaitForSeconds(lengthAudio);
        Debug.Log("WaitAudioToBeFinishedAndTeleportToNextScene");
        OculusCameraFade.Instance.DoFade(() => {
            SceneManager.LoadScene(3);
        });
    }

    public void teleportToTheNextScene()
    {
        player.setEnabled(false);
        float lengthAudio = audioManager.PlayRightCrossing();
        StartCoroutine(WaitAudioToBeFinishedAndTeleportToNextScene(lengthAudio));
    }

    public void manageWrongChoiceInHub()
    {
        hub.WrongChoiceMade();
        player.setEnabled(false);
        float lengthAudio = audioManager.PlayWrongChoiceHub();
        StartCoroutine(WaitAudioToBeFinishedAndEnablePlayer(lengthAudio));

    }

    public void manageRightChoiceInHub()
    {
        hub.RightChoiceMade();
        player.setEnabled(false);
        float lengthAudio = audioManager.PlayRightChoiceHub();
        StartCoroutine(WaitAudioToBeFinishedAndTeleportToCrossing(lengthAudio));

    }


    // Update is called once per frame
    void Update()
    {

    }
}
