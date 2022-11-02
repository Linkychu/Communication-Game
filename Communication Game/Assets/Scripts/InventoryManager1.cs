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
    public List<ItemClass> player1KeyItems = new List<ItemClass>();

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

    [Header("KeyItemsUI")] 
    [SerializeField] private GameObject keyItemsUIObj;

    [SerializeField] private Image KeyItemsImage;

    private ItemClass currentSelectedKeyItem;
    private bool firstKeyItem;
    private int currentSelectedKeyItemIndex;

    
    private PlayerClass player;
    private void Awake()
    {
        instance = this;
        playerInput = new Player1();
        player = GetComponentInChildren<PlayerClass>();
        playerInput.Inventory.OpenInventory.performed += OpenInventory;
        _movement = transform.GetComponentInChildren<PlayerOneMovement>();
        playerInput.Inventory.Interact.performed += ActionPerformed;
        playerInput.Inventory.SwapKeyItem.performed += SwapKeyItem;
        playerInput.Inventory.UseKeyItem.performed += UseKeyItem;
        screenWidth = canvas.worldCamera.pixelWidth;
        ResetInventory();
    }

    private void ActionPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("Pressed");
        Collider[] chests =
            Physics.OverlapSphere(player.transform.position, 5, GlobalInventoryManager.instance.chestLayer, QueryTriggerInteraction.Collide);
        if (chests.Length > 0)
        {
            for (int i = 0; i < chests.Length; i++)
            {
                Debug.Log("Worked");
                var pm = player.transform.parent.GetComponent<PlayerManager>();
                Debug.Log("Recieved");
                chests[i].GetComponent<OpenItem>().ActionPerformed(1, pm);

            }
            
        }

        else
        {
            return;
        }
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
        keyItemsUIObj.SetActive(false);
        currentSelectedKeyItemIndex = 0;
        currentSelectedKeyItem = null;
        UpdateInventorySlots();

    }
    

    public void AddItem(ItemClass item, int amount)
    {
        switch (item.type)
        {
            case ItemType.Normal:

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
                break;
            case ItemType.Key:
                if (player1InventoryRef.ContainsKey(item))
                {
                    if (player1KeyItems.Contains(item))
                    {
                       return;
                    }

                    player1Inventory.Add(item);
                    player1InventoryRef[item] = amount;
                }

                else
                {
                    
                    player1InventoryRef.Add(item, amount);
                    player1KeyItems.Add(item);
                    
                    
                    if (firstKeyItem == false)
                    {
                        firstKeyItem = true;
                        keyItemsUIObj.SetActive(true);
                        LoadKeyItem();  
                    }
                    

                }
            
                player1InventoryRef[item] = Mathf.Clamp(player1InventoryRef[item], 0, 1);
                break;
            default:
                break;
        }
        
        UpdateInventorySlots();
    }
    
    

    public void SubtractItem(ItemClass item, int amount)
    {
        switch (item.type)
        {
            case ItemType.Normal:
                if(!(player1InventoryRef.ContainsKey(item) && player1Inventory.Contains(item) ))
                    return;
                player1InventoryRef[item] -= amount;
        
                player1InventoryRef[item] = Mathf.Clamp(player1InventoryRef[item], 0, 99);

                if (player1InventoryRef[item] <= 0)
                {
                    player1Inventory.Remove(item);
                }

                break;
            case ItemType.Key:
                if(!(player1InventoryRef.ContainsKey(item) && player1KeyItems.Contains(item) ))
                    return;
                player1InventoryRef[item] -= amount;
        
                player1InventoryRef[item] = Mathf.Clamp(player1InventoryRef[item], 0, 1);

                if (player1InventoryRef[item] <= 0)
                {
                    player1KeyItems.Remove(item);
                }
                break;
            default:
                return;
            
                

        }
      
        
        
       
        UpdateInventorySlots();
        LoadKeyItem();
    }

    public bool HasEnoughItem(ItemClass item, int amount)
    {
        if (!player1InventoryRef.ContainsKey(item))
            return false;
        if (player1InventoryRef[item] < 0)
            return false;
        return player1InventoryRef[item] >= amount;
        
    }

    public void SwapKeyItem(InputAction.CallbackContext context)
    {
        Debug.Log("Pressed");
        if(player1KeyItems.Count == 0)
            return;
        if (currentSelectedKeyItemIndex >= player1KeyItems.Count)
        {
            currentSelectedKeyItemIndex = 0;
        }
        else
        {
            currentSelectedKeyItemIndex += 1;
        }
       
        LoadKeyItem(); 
    }

    void LoadKeyItem()
    {
        currentSelectedKeyItem = player1KeyItems[currentSelectedKeyItemIndex];
        KeyItemsImage.sprite = currentSelectedKeyItem.image;
    }

    void UseKeyItem(InputAction.CallbackContext context)
    {
        currentSelectedKeyItem.UseItem(null, player);
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