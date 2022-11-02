using System;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace General
{
    public class BossStairTrigger : MonoBehaviour
    {
        private bool isPlayer1;
        private bool isPlayer2;

        private bool playerFound;

        private GameObject[] playerSpawnPos;

        private GameObject bossroom;

        void CheckForUpdate()
        {
            if (!isPlayer1 || !isPlayer2) return;
            GameObject generator = GameObject.FindWithTag("Generator");
            generator.SetActive(false);
            bossroom.SetActive(true);
            PlayerClass[] players = FindObjectsOfType<PlayerClass>();
            playerSpawnPos = GameObject.FindGameObjectsWithTag("Spawn");
            for (int i = 0; i < playerSpawnPos.Length; i++)
            {
                players[i].transform.position = playerSpawnPos[i].transform.position;
            }
        }

        private void Awake()
        {
            bossroom = GameObject.FindWithTag("BossGenerator");
            bossroom.SetActive(false);
        }

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
}