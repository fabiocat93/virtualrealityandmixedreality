using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal5 : MonoBehaviour
{
    public Crossing5 destination;
    public bool isRight;
    public GameController5 gameController;
    private Hub5 hub;

    private void Start()
    {
        hub = GetComponentInParent<Hub5>();
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
