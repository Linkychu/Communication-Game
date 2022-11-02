using Characters;
using UnityEngine;

namespace Items
{
    public class PotionScript : ItemBase
    {
        public int amount;
        public override void UseItem(CharacterClass user, PlayerClass player)
        {
            player.Heal(amount);
        }
    }
}