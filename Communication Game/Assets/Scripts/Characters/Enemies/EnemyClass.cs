using Characters;
using Player;
using UnityEngine;

namespace Enemy
{
    public class EnemyClass : CharacterClass
    {
        
        private void Awake()
        {
        
            values = (CharacterValues) Instantiate(values);
            charBase = (CharacterBase) Instantiate(charBase);
        }
        public int expAmount;
        public override void Death()
        {
            PlayerClass[] players = FindObjectsOfType<PlayerClass>();
            players[0].level.AddExp(Mathf.FloorToInt((expAmount * charBase.BaseHP * values.myStats.level) / (7f * PlayerStateManager.instance.alivePlayers.Count)));
            players[1].level.AddExp(Mathf.FloorToInt((expAmount * charBase.BaseHP * values.myStats.level) / (7f * PlayerStateManager.instance.alivePlayers.Count)));

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}