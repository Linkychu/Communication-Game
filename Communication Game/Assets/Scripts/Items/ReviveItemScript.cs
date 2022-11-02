using UnityEngine;
using Characters;


namespace Items
{
    public class ReviveItemScript : ItemBase
    {
        public int amount = 50;
        public override void UseItem(CharacterClass user, PlayerClass player)
        {
            player.Revive(amount);
        }

        
    }
}