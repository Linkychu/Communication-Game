using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager1 : MonoBehaviour
{
    public static InventoryManager1 instance { get; set; }

    private Dictionary<ItemClass, int> player1InventoryRef = new Dictionary<ItemClass, int>();

    public List<ItemClass> player1Inventory = new List<ItemClass>();

    private bool canOpenInventory;

    private Player1 playerInput;

    private bool inventoryOpen;

    private PlayerOneMovement _movement;

    public Canvas canvas;

    public Transform inventorySlotManager;

    private float screenWidth;

    [Header("InventoryScreen")]
    private Button[] inventorySlots;
    private void Awake()
    {
        instance = this;
        playerInput = new Player1();
        playerInput.Inventory.OpenInventory.performed += OpenInventory;
        _movement = transform.GetComponentInChildren<PlayerOneMovement>();
        screenWidth = canvas.worldCamera.pixelWidth;
        ResetInventory();
    }

    private void Update()
    {
        if (canOpenInventory && inventoryOpen)
        {
            float x = playerInput.Inventory.MousePosition.ReadValue<Vector2>().x;

            if (x > screenWidth)
            {
                Cursor.visible = false;
            }

            else
            {
                Cursor.visible = true;
            }
        }
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
        player1InventoryRef.Clear();
        player1Inventory.Clear();
    }

    private void Start()
    {
        canOpenInventory = true;
        inventoryOpen = canvas.gameObject.activeInHierarchy;
    }

    public void AddItem(ItemClass item, int amount)
    {
        if (player1InventoryRef.ContainsKey(item))
        {
            if (player1Inventory.Contains(item))
            {
                player1InventoryRef[item] += amount;
            }

            else
            {
                player1Inventory.Add(item);
                player1InventoryRef[item] = amount;
            }
        }

        else
        {
            player1InventoryRef.Add(item, amount);
            player1Inventory.Add(item);
            
        }

        player1InventoryRef[item] = Mathf.Clamp(player1InventoryRef[item], 0, 99);

    }

    public void SubtractItem(ItemClass item, int amount)
    {
        if(!(player1InventoryRef.ContainsKey(item) && player1Inventory.Contains(item)))
            return;
        
        
        player1InventoryRef[item] -= amount;
        
        player1InventoryRef[item] = Mathf.Clamp(player1InventoryRef[item], 0, 99);

        if (player1InventoryRef[item] <= 0)
        {
            player1Inventory.Remove(item);
        }
    }


    void OpenInventory(InputAction.CallbackContext context)
    {
        inventoryOpen = !inventoryOpen;

        switch (inventoryOpen)
        {
            case true:
                _movement.canMove = false;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                _movement.DisableCameraInput();
                break;
            default:
                _movement.canMove = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                _movement.EnableCameraInput();
                break;
                
        }
        
        canvas.gameObject.SetActive(inventoryOpen);
        
    }
    
    
}