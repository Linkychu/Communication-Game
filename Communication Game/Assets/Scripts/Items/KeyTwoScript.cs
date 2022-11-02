using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;

public class KeyTwoScript : KeyItemBase
{
    private BossKeyCheckScript bossDoor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void UseItem(CharacterClass Class, PlayerClass player)
    {
        bossDoor = GameObject.FindWithTag("BossKeyDoor").GetComponent<BossKeyCheckScript>();
        bossDoor.hasTwoKey = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
