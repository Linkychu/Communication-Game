using System;
using System.Collections.Generic;
using General;
using UnityEngine;

namespace Player
{
    public class PlayerStateManager : MonoBehaviour
    {
        
        public static PlayerStateManager instance { get; set; }

        [HideInInspector]
        public PlayerClass[] players = new PlayerClass[2];
        [HideInInspector]
        public List<PlayerClass> alivePlayers = new List<PlayerClass>();

        private int firstPlayerId = -1;
        private int secondPlayerId = -1;

        private void Awake()
        {
            instance = this;
            firstPlayerId = PlayerPrefs.GetInt("FirstPlayer");
            secondPlayerId = PlayerPrefs.GetInt("SecondPlayer");
            SpawnPlayers();
           
            
        }

        void SpawnPlayers()
        {
            var Player1 = Instantiate(GameManager.instance.slottedPlayers[firstPlayerId].model1, Vector3.zero, GameManager.instance.slottedPlayers[firstPlayerId].model1.transform.rotation, transform);
            var Player2 = Instantiate(GameManager.instance.slottedPlayers[secondPlayerId].model2, Vector3.zero, GameManager.instance.slottedPlayers[secondPlayerId].model2.transform.rotation, transform);
            Load();
            
        }
        

        private void Load()
        {

            players = FindObjectsOfType<PlayerClass>();
            alivePlayers.Add(players[0]);
            alivePlayers.Add(players[1]);
        }
    }
}