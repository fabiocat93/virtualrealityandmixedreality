using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal4 : MonoBehaviour
{
    public Crossing4 destination;
    public bool isRight;
    public GameController4 gameController;
    public AudioClip musicAudioClip;

    private Hub4 hub;
    private Player4 player;

    private void Start()
    {
        hub = GetComponentInParent<Hub4>();
        player = FindObjectOfType<Player4>();
    }

    public void EnterPortal()
    {
        if (!isRight)
        {
            // Logica di quando viene sbagliata una scelta
            gameController.manageWrongChoiceInHub();
            return;
        }
        else
        {
            player.canSelect = false;
            // Logica di quando viene fatta la scelta giusta e bisogna scendere all'incrocio
            gameController.manageRightChoiceInHub();
        }
    }

    public void PlayAudioClip()
    {
        if (gameController.audioManager.WhatIsBeingPlayed() != musicAudioClip && player.canSelect)
        {
            gameController.audioManager.PlayMusicClip(musicAudioClip);
        }
    }
}
