using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController5 : MonoBehaviour
{

    public Hub5 hub;
    public Crossing5 crossing;
    public AudioManager5 audioManager;

    private Player5 player;
     
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player5>();
        Debug.Log("dimmi");
        player.setEnabled(true);
        float lengthAudio = audioManager.PlayStartHub();

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

            Semaphore4 semaphore = crossing.GetComponentInChildren<Semaphore4>();
            if (semaphore != null)
            {
                semaphore.isOn = true;
            }

            player.setEnabled(true);
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
        Semaphore4 semaphore = crossing.GetComponentInChildren<Semaphore4>();
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
            SceneManager.LoadScene(4);
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
