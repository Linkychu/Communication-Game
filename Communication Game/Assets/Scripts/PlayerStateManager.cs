using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerStateManager : MonoBehaviour
    {
        
        public static PlayerStateManager instance { get; set; }
        
        
        public List<PlayerClass> alivePlayers = new List<PlayerClass>();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
        
            PlayerClass[] players = FindObjectsOfType<PlayerClass>();
            alivePlayers.Add(players[0]);
            alivePlayers.Add(players[1]);
         }
    }
}