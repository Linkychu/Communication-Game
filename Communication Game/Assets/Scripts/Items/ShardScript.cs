using Characters;
using UnityEngine;

namespace Items
{
    public enum Shard
    {
        Attack,
        Defence,
        SpecialA,
        SpecialD
    };
    public class ShardScript : ItemBase
    {
        public int modifier;
        public Shard shard;
            
        public override void UseItem(CharacterClass user, PlayerClass player)
        {
            switch (shard)
            {
                case Shard.Attack:
                    player.BoostStats(modifier, StatBoost.Attack);
                    break;
                case Shard.Defence:
                    player.BoostStats(modifier, StatBoost.Defence);
                    break;
                case Shard.SpecialA:
                    player.BoostStats(modifier, StatBoost.SpecialAttack);
                    break;
                case Shard.SpecialD:
                    player.BoostStats(modifier, StatBoost.SpecialDefence);
                    break;
                
            }
        }
    }
}