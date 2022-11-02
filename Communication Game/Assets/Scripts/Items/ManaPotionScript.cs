using Characters;
using UnityEngine;

namespace Items
{
    public class ManaPotionScript : ItemBase
    {
        public int amount;
        public override void UseItem(CharacterClass user, PlayerClass player)
        {
            player.HealMana(amount);
        }
    }
}