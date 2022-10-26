using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GlobalInventoryManager : MonoBehaviour
{
    public static GlobalInventoryManager instance { get; set; }
    public InventoryManager1 p1;

    public InventoryManager2 p2;

    private void Awake()
    {
        instance = this;
    }

    public void SendItem(ItemClass item, int amount,  PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.Player1:
                p2.AddItem(item, amount);
                p1.SubtractItem(item, amount);
                break;
            case PlayerState.Player2:
                p1.AddItem(item, amount);
                p2.SubtractItem(item, amount);
                break;
            
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
