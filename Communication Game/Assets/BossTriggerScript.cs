using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTriggerScript : MonoBehaviour
{
    private bool isPlayer1;
    private bool isPlayer2;

    public EventHandler OnPlayerEnterTrigger;

    private void OnTriggerEnter(Collider other)
    {
            
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerClass>().playerState == PlayerState.Player1)
            {
                isPlayer1 = true;
            }

            if (other.GetComponent<PlayerClass>().playerState == PlayerState.Player2)
            {
                isPlayer2 = true;
            }
                
            CheckForUpdate();
        }
            
            

    }

    private void CheckForUpdate()
    {
       OnPlayerEnterTrigger.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerClass>().playerState == PlayerState.Player1)
            {
                isPlayer1 = false;
            }

            if (other.GetComponent<PlayerClass>().playerState == PlayerState.Player2)
            {
                isPlayer2 = false;
            }
        }
    }
}

