using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager4 : MonoBehaviour
{
    private AudioSource myAudioSource;

    public AudioClip startHub;
    public AudioClip rightChoiceHub;
    public AudioClip wrongChoiceHub;
    public AudioClip startCrossing;
    public AudioClip rightCrossing;
    public AudioClip wrongCrossingRed;
    public AudioClip wrongCrossingMusic;
    public AudioClip wrongCrossingStrips;
    public AudioClip wrongIncident;
    public AudioClip hakunaMatata;
    public AudioClip theLionSleepTonight;
    public AudioClip iWillSurvive;

    public Player4 player;

    private int selectedSong = -1;
    private AudioClip selectedMusicClip;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player4>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlayMusic(AudioClip currentAudio)
    {
        Debug.Log("AudioManager - PlayMusic - riproduco: " + currentAudio);
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.clip = currentAudio;
        myAudioSource.Play();
    }

    public float PlayMusicClip(AudioClip musicClip)
    {
        PlayMusic(musicClip);
        selectedMusicClip = musicClip;
        return musicClip.length;
    }

    public float PlayIWillSurvive()
    {
        PlayMusic(iWillSurvive);
        player.canSelect = true;
        selectedSong = 0;
        return iWillSurvive.length;
    }

    public float PlayHakunaMatata()
    {
        PlayMusic(hakunaMatata);
        player.canSelect = true;
        selectedSong = 1;
        return hakunaMatata.length;
    }

    public float PlayTheLionSleepTOnight()
    {
        PlayMusic(theLionSleepTonight);
        player.canSelect = true;
        selectedSong = 2;
        return theLionSleepTonight.length;
    }

    public float PlaySelectedSong()
    {
        /*switch (selectedSong)
        {
            case 0: return PlayIWillSurvive();
            case 1: return PlayHakunaMatata();
            case 2: return PlayTheLionSleepTOnight();
            default: return -42.0f;
        }*/

        if (selectedMusicClip != null)
        {
            return PlayMusicClip(selectedMusicClip);
        }

        return -42.0f;
    }

    public float PlayWrongIncident()
    {
        PlayMusic(wrongIncident);
        return wrongIncident.length;

    }

    public float PlayWrongCrossingRed()
    {
        PlayMusic(wrongCrossingRed);
        return wrongCrossingRed.length;

    }

    public float PlayWrongCrossingMusic()
    {
        PlayMusic(wrongCrossingMusic);
        return wrongCrossingMusic.length;
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

    public AudioClip WhatIsBeingPlayed()
    {
        if (myAudioSource.isPlaying)
            return myAudioSource.clip;
        else
            return null;
    }

    public bool IsPlayingTheLionSleepTonight()
    {
        return WhatIsBeingPlayed() == theLionSleepTonight;
    }

    public bool IsPlayingHakunaMatata()
    {
        return WhatIsBeingPlayed() == hakunaMatata;
    }

    public bool IsPlayingIWillSurvive()
    {
        return WhatIsBeingPlayed() == iWillSurvive;
    }
}