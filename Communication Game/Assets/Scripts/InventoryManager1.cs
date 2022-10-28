using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager1 : MonoBehaviour
{
    public static InventoryManager1 instance { get; set; }

    private Dictionary<ItemClass, int> player1InventoryRef = new Dictionary<ItemClass, int>();

    public List<ItemClass> player1Inventory = new List<ItemClass>();

    private bool canOpenInventory;

    [HideInInspector]
    public Player1 playerInput;

    private bool inventoryOpen;

    private PlayerOneMovement _movement;

    public Canvas canvas;
    
    private float screenWidth;

    [SerializeField] private Sprite defaultSprite;
    [Header("InventoryScreen")]
    public List<Button> inventorySlots = new List<Button>();

    public bool isInteracting;

    

    private void Awake()
    {
        instance = this;
        playerInput = new Player1();
        playerInput.Inventory.OpenInventory.performed += OpenInventory;
        _movement = transform.GetComponentInChildren<PlayerOneMovement>();
        playerInput.Inventory.Interact.performed += context => isInteracting = true;
        playerInput.Inventory.Interact.canceled += context => isInteracting = false;
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

    public void CloseInventory()
    {
        inventoryOpen = false;
        _movement.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canvas.gameObject.SetActive(inventoryOpen);
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

    public bool HasEnoughItem(ItemClass item, int amount)
    {
        if (!player1InventoryRef.ContainsKey(item))
            return false;
        if (player1InventoryRef[item] < 0)
            return false;
        return player1InventoryRef[item] >= amount;
        
    }
    
    
    


    void UpdateInventorySlots()
    {

        foreach (var button in inventorySlots)
        {
            button.GetComponent<Image>().sprite = defaultSprite;
            button.interactable = false;
            button.GetComponentInChildren<TextMeshProUGUI>().text = String.Empty;
        }
        if (player1Inventory.Count > 0)
        {
            for (int i = 0; i < player1Inventory.Count; i++)
            {
                inventorySlots[i].GetComponent<Image>().sprite = player1Inventory[i].image;
                inventorySlots[i].GetComponentInChildren<TextMeshProUGUI>().text = player1InventoryRef[player1Inventory[i]].ToString();
                inventorySlots[i].interactable = true;

            }
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
        
        UpdateInventorySlots();
        canvas.gameObject.SetActive(inventoryOpen);
        
    }
    
    
}