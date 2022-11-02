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
            PlayerClass[] players = PlayerStateManager.instance.alivePlayers.ToArray();
            Debug.Log(players[0]);
            Debug.Log(players[1]);

            for (int i = 0; i < players.Length; i++)
            {
                players[i].level.AddExp(Mathf.FloorToInt((expAmount * charBase.BaseHP * values.myStats.level) / (7f * PlayerStateManager.instance.alivePlayers.Count)));
            }

            
            Destroy(gameObject);
        }
    }
}