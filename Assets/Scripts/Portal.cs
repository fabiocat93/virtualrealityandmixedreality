using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Crossing destination;
    public bool isRight;
    public GameController gameController;
    private Hub hub;

    private void Start()
    {
        hub = GetComponentInParent<Hub>();
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
