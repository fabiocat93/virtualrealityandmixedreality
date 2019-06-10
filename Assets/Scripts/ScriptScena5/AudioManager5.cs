using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager5 : MonoBehaviour
{
    private AudioSource myAudioSource;

    public AudioClip startHub;
    public AudioClip rightChoiceHub;
    public AudioClip wrongChoiceHub;
    public AudioClip startCrossing;
    public AudioClip rightCrossing;
    public AudioClip wrongCrossingRed;
    public AudioClip wrongCrossingStrips;
    public AudioClip hakunaMatata;
    public AudioClip theLionSleepTonight;
    public AudioClip iWillSurvive;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StopMusic()
    {
        Debug.Log("stop");
    }
     

    void PlayMusic(AudioClip currentAudio)
    {
        Debug.Log("AudioManager - PlayMusic - riproduco: " + currentAudio);
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.clip = currentAudio;
        myAudioSource.Play();
    }

    public float PlayIWillSurvive()
    {
        PlayMusic(iWillSurvive);
        return iWillSurvive.length;
    }

    public float PlayHakunaMatata()
    {
        PlayMusic(hakunaMatata);
        return hakunaMatata.length;
    }

    public float PlayTheLionSleepTOnight()
    {
        PlayMusic(theLionSleepTonight);
        return theLionSleepTonight.length;
    }

    public float PlayWrongCrossingRed()
    {
        PlayMusic(wrongCrossingRed);
        return wrongCrossingRed.length;

    }

    public float PlayRightCrossing()
    {
        PlayMusic(rightCrossing);
        return rightCrossing.length;

    }

    public float PlayStartCrossing()
    {
        PlayMusic(startCrossing);
        return startCrossing.length;

    }

    public float PlayWrongChoiceHub()
    {
        PlayMusic(wrongChoiceHub);
        return wrongChoiceHub.length;

    }

    public float PlayRightChoiceHub()
    {
        PlayMusic(rightChoiceHub);
        return rightChoiceHub.length;

    }

    public float PlayStartHub()
    {
        PlayMusic(startHub);
        return startHub.length;

    }

    public float PlayWrongCrossingStrips()
    {
        PlayMusic(wrongCrossingStrips);
        return wrongCrossingStrips.length;
    }
}