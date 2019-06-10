using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal2 : MonoBehaviour
{
    public Crossing2 destination;
    public bool isRight;
    public GameController2 gameController;
    private Hub2 hub;

    private void Start()
    {
        hub = GetComponentInParent<Hub2>();
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
            // Logica di quando viene fatta la scelta giusta e bisogna scendere all'incrocio
            gameController.manageRightChoiceInHub();
        }
    }
}
