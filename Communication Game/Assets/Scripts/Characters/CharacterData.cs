using Player;
using UnityEngine;
using UnityEngine.AI;

namespace General
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Data/CharacterData", order = 0)]
    public class CharacterData : ScriptableObject
    {
        public string Name;
        public Sprite image;
        public GameObject model1;
        public GameObject model2;

    }
}