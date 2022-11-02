using System;
using Characters;
using UnityEngine;

namespace Items
{
    public class MapScript : KeyItemBase
    {
        public GameObject Map;
        private bool isMapUsed;

        private void Start()
        {
            Map.SetActive(isMapUsed);
        }

        public override void UseItem(CharacterClass Class, PlayerClass player)
        {
            Map = player.map;
            isMapUsed = !isMapUsed;
            Map.SetActive(isMapUsed);
        }
    }
}