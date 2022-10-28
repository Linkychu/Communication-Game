using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GlobalInventoryManager : MonoBehaviour
{
    public static GlobalInventoryManager instance { get; set; }
    public InventoryManager1 p1;

    public InventoryManager2 p2;
    public TextAsset receivingText;
    public TextAsset tradedText;
    public TextAsset droppedText;
    public delegate void InputDisplay();
    
    
    private void Awake()
    {
        instance = this;
    }

    public void SendItem(ItemClass item, int amount,  PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.Player1:
                p1.AddItem(item, amount);
                p2.SubtractItem(item, amount);
                DialogueManager.instance.p2.TradedItem(tradedText, "Player 1", item.name, amount);
                DialogueManager.instance.p1.TradedItem(receivingText, "Player 2", item.name, amount);
                
                break;
            case PlayerState.Player2:
                p2.AddItem(item, amount);
                p1.SubtractItem(item, amount);
                DialogueManager.instance.p1.TradedItem(tradedText, "Player 2", item.name, amount);
                DialogueManager.instance.p2.TradedItem(receivingText, "Player 1", item.name, amount);
                break;
            
        }
        
        
    }

    public void DropItem(ItemClass item, int amount, PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Player1:
                DialogueManager.instance.p1.DisplayNewItem(droppedText, item.name, amount);
                break;
            case PlayerState.Player2:
                DialogueManager.instance.p2.DisplayNewItem(droppedText, item.name, amount);
                break;
        }
    }
    void Start()    
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
