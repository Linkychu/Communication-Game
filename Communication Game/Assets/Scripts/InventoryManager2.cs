using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryManager2 : MonoBehaviour
{
    public static InventoryManager2 instance { get; set; }

    private Dictionary<ItemClass, int> player2InventoryRef = new Dictionary<ItemClass, int>();

    public List<ItemClass> player2Inventory = new List<ItemClass>();

    private bool canOpenInventory;

    private Player2 playerInput;

    private bool inventoryOpen;

    private PlayerTwoMovement _movement;

    public Canvas canvas;

    public Transform inventorySlotManager;

   

    [Header("InventoryScreen")]
    private Button[] inventorySlots;
    private void Awake()
    {
        instance = this;
        playerInput = new Player2();
        playerInput.Inventory.OpenInventory.performed += OpenInventory;
        _movement = transform.GetComponentInChildren<PlayerTwoMovement>();
        ResetInventory();
    }

    private void Update()
    {
        
        
    }

    private void OnEnable()
    {
        playerInput.Inventory.Enable();
    }

    private void OnDisable()
    {
        playerInput.Inventory.Disable();
    }

    void ResetInventory()
    {
        player2InventoryRef.Clear();
        player2Inventory.Clear();
    }

    private void Start()
    {
        canOpenInventory = true;
        inventoryOpen = canvas.gameObject.activeInHierarchy;
    }

    public void AddItem(ItemClass item, int amount)
    {
        if (player2InventoryRef.ContainsKey(item))
        {
            if (player2Inventory.Contains(item))
            {
                player2InventoryRef[item] += amount;
            }

            else
            {
                player2Inventory.Add(item);
                player2InventoryRef[item] = amount;
            }
        }

        else
        {
            player2InventoryRef.Add(item, amount);
            player2Inventory.Add(item);
            
        }

        player2InventoryRef[item] = Mathf.Clamp(player2InventoryRef[item], 0, 99);

    }

    public void SubtractItem(ItemClass item, int amount)
    {
        if(!(player2InventoryRef.ContainsKey(item) && player2Inventory.Contains(item)))
            return;
        
        
        player2InventoryRef[item] -= amount;
        
        player2InventoryRef[item] = Mathf.Clamp(player2InventoryRef[item], 0, 99);

        if (player2InventoryRef[item] <= 0)
        {
            player2Inventory.Remove(item);
        }
    }


    void OpenInventory(InputAction.CallbackContext context)
    {
        inventoryOpen = !inventoryOpen;

        switch (inventoryOpen)
        {
            case true:
                _movement.canMove = false;
                _movement.DisableCameraInput();
                break;
            default:
                _movement.canMove = true;
                _movement.EnableCameraInput();
                break;
                
        }
        
        canvas.gameObject.SetActive(inventoryOpen);
        
    }
    
    
}