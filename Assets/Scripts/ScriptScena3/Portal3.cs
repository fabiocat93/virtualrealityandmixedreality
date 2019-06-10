using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal3 : MonoBehaviour
{
    public Crossing3 destination;
    public bool isRight;
    public GameController3 gameController;
    private Hub3 hub;

    private void Start()
    {
        hub = GetComponentInParent<Hub3>();
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
