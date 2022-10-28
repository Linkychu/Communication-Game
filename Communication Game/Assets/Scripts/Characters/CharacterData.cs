using Player;
using UnityEngine;
using UnityEngine.AI;

namespace General
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Data/CharacterData", order = 0)]
    public class CharacterData : ScriptableObject
    {
        public GameObject model;
        public CharacterBase CharacterBase;
        public CharacterValues values;
        public int MinLevel;
        public int MaxLevel;

        public int Level;

        
        


        public void Initialise()
        {
            CharacterBase = (CharacterBase) Instantiate(CharacterBase);
            Level = values.myStats.level = Random.Range(MinLevel, MaxLevel + 1);
            //FindObjectOfType<ChooseCharacterScript>().DisplayStats();
        }
       
    }
}