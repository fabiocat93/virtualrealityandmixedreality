using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public Hub hub;
    public Crossing crossing;
    public AudioManager audioManager;
    private Player player;
    public GameObject OVRCamera;
    public Transform cameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        player.setEnabled(false);
        float lengthAudio = audioManager.PlayIntro();
        Debug.Log(lengthAudio);
        StartCoroutine(WaitAudioToBeFinishedAndMovePlayerToHub(lengthAudio));
        
    }

    public IEnumerator WaitAudioToBeFinishedAndMovePlayerToHub(float lengthAudio)
    {
        yield return new WaitForSeconds(lengthAudio);
        Debug.Log("WaitAudioToBeFinishedAndMovePlayerToHub");
        player.setEnabled(true);
        Vector3 camHubPosition = cameraPosition.position;
        OVRCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
        Debug.Log(camHubPosition);
        OVRCamera.transform.position = new Vector3(camHubPosition.x, camHubPosition.y, camHubPosition.z);
        Debug.Log(OVRCamera.transform.position);

        float lengthAudio2 = audioManager.PlayStartHub();
        StartCoroutine(WaitAudioToBeFinishedAndEnablePlayer(lengthAudio2));

    }


    public IEnumerator WaitAudioToBeFinishedAndEnablePlayer(float lengthAudio)
    {
        yield return new WaitForSeconds(lengthAudio);
        Debug.Log("WaitAndStartChoiceInHub");
        player.setEnabled(true);
       
    }

    public IEnumerator WaitAudioToBeFinishedAndTeleportToCrossing(float lengthAudio)
    {
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

            Semaphore semaphore = crossing.GetComponentInChildren<Semaphore>();
            if (semaphore != null)
            {
                semaphore.isOn = true;
            }

            player.setEnabled(false);
            float lengthAudio2 = audioManager.PlayStartCrossing();
            StartCoroutine(WaitAudioToBeFinishedAndEnablePlayer(lengthAudio2));
        });
    }

    public IEnumerator WaitAudioToBeFinishedAndRestartLevel(float lengthAudio)
    {
        yield return new WaitForSeconds(lengthAudio);
        Debug.Log("WaitAudioToBeFinishedAndRestartLevel");
        player.Lose();
        player.setEnabled(true);
        Semaphore semaphore = crossing.GetComponentInChildren<Semaphore>();
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
            SceneManager.LoadScene(1);
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
