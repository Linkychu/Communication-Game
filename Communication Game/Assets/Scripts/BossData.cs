using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;


[CreateAssetMenu(fileName = "Boss", menuName = "Data/Boss", order = 0)]
public class BossData : ScriptableObject
{

    public CharacterBase charBase;

    public CharacterValues values;

    public Moves[] moves;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
