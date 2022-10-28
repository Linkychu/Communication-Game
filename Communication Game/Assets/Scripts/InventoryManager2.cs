using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManager2 : MonoBehaviour
{
    public static InventoryManager2 instance { get; set; }

    private Dictionary<ItemClass, int> player2InventoryRef = new Dictionary<ItemClass, int>();

    public List<ItemClass> player2Inventory = new List<ItemClass>();

    private bool canOpenInventory;

    [HideInInspector]
    public Player2 playerInput;

    private bool inventoryOpen;

    private PlayerTwoMovement _movement;

    public Canvas canvas;

    public EventSystem eventSystem;

    public bool isInteracting;

    public Sprite defaultSprite;
    
    [Header("InventoryScreen")]
    public List<Button> inventorySlots = new List<Button>();
    
    
    private void Awake()
    {
        instance = this;
        playerInput = new Player2();
        playerInput.Inventory.OpenInventory.performed += OpenInventory;
        _movement = transform.GetComponentInChildren<PlayerTwoMovement>();
        playerInput.Inventory.Interact.performed += context => isInteracting = true;
        playerInput.Inventory.Interact.canceled += context => isInteracting = false;
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
        UpdateInventorySlots();
    }

    public void AddItem(ItemClass item, int amount)
    {
        Debug.Log(item);
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
    
    public bool HasEnoughItem(ItemClass item, int amount)
    {
        if (!player2InventoryRef.ContainsKey(item))
            return false;
        if (player2InventoryRef[item] < 0)
            return false;
        return player2InventoryRef[item] >= amount;
        
    }


    public void CloseInventory()
    {
        inventoryOpen = false;
        _movement.canMove = true;
        canvas.gameObject.SetActive(inventoryOpen);
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
        
        UpdateInventorySlots();
        canvas.gameObject.SetActive(inventoryOpen);
        eventSystem.SetSelectedGameObject(inventorySlots[0].gameObject);
        
    }
    
     void UpdateInventorySlots()
        {
    
            foreach (var button in inventorySlots)
            {
                button.GetComponent<Image>().sprite = defaultSprite;
                button.interactable = false;
                button.GetComponentInChildren<TextMeshProUGUI>().text = String.Empty;
            }
            if (player2Inventory.Count > 0)
            {
                for (int i = 0; i < player2Inventory.Count; i++)
                {
                    inventorySlots[i].GetComponent<Image>().sprite = player2Inventory[i].image;
                    inventorySlots[i].GetComponentInChildren<TextMeshProUGUI>().text = player2InventoryRef[player2Inventory[i]].ToString();
                    inventorySlots[i].interactable = true;
    
                }
            }
        }
    
    
}