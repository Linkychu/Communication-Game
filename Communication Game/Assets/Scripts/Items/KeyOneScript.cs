using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;

public class KeyOneScript : KeyItemBase
{
    private BossKeyCheckScript bossDoor;
    // Start is called before the first frame update
    void Start()
    {
        bossDoor = FindObjectOfType<BossKeyCheckScript>();
    }

    public override void UseItem(CharacterClass Class, PlayerClass player)
    {
        bossDoor = GameObject.FindWithTag("BossKeyDoor").GetComponent<BossKeyCheckScript>();
        bossDoor.hasOneKey = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
