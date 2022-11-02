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
    public List<ItemClass> player2KeyItems = new List<ItemClass>();

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
    
    [Header("KeyItemsUI")] 
    [SerializeField] private GameObject keyItemsUIObj;

    [SerializeField] private Image KeyItemsImage;

    private ItemClass currentSelectedKeyItem;
    private bool firstKeyItem;
    private int currentSelectedKeyItemIndex;
    
    private PlayerClass player;
    [SerializeField]private LayerMask chestLayer;
    
    private void Awake()
    {
        instance = this;
        playerInput = new Player2();
        playerInput.Inventory.OpenInventory.performed += OpenInventory;
        player = GetComponentInChildren<PlayerClass>();
        _movement = transform.GetComponentInChildren<PlayerTwoMovement>();
        playerInput.Inventory.Interact.performed += ActionPerformed;
        playerInput.Inventory.SwapKeyItem.performed += SwapKeyItem;
        playerInput.Inventory.UseKeyItem.performed += UseKeyItem;
        ResetInventory();
    }

    private void ActionPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("Pressed");
        Collider[] chests =
            Physics.OverlapSphere(player.transform.position, 5, GlobalInventoryManager.instance.chestLayer, QueryTriggerInteraction.Collide);
        if (chests.Length > 0)
        {
            Debug.Log("Worked");
            if (chests[0].transform.TryGetComponent(out OpenItem item))
            {
                var pm = player.transform.parent.GetComponent<PlayerManager>();
                Debug.Log("Recieved");
                item.ActionPerformed(2, pm);
            }

            else
            {
                return;
            }
        }
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
                break;
            case ItemType.Key:
                if (player2InventoryRef.ContainsKey(item))
                {
                    if (player2KeyItems.Contains(item))
                    {
                        return;
                    }

                    player2Inventory.Add(item);
                    player2InventoryRef[item] = amount;
                }

                else
                {
                    
                    player2InventoryRef.Add(item, amount);
                    player2KeyItems.Add(item);
                    
                    
                    if (firstKeyItem == false)
                    {
                        firstKeyItem = true;
                        keyItemsUIObj.SetActive(true);
                        LoadKeyItem();  
                    }
                    

                }
            
                player2InventoryRef[item] = Mathf.Clamp(player2InventoryRef[item], 0, 1);
                break;
            default:
                break;
        }
        
        UpdateInventorySlots();
    }

    public void SubtractItem(ItemClass item, int amount)
    {
        if(item == null)
            return;
        switch (item.type)
        {
            case ItemType.Normal:
                if ((player2InventoryRef.ContainsKey(item) && player2Inventory.Contains(item)))
                {
                    player2InventoryRef[item] -= amount;

                    player2InventoryRef[item] = Mathf.Clamp(player2InventoryRef[item], 0, 99);

                    if (player2InventoryRef[item] <= 0)
                    {
                        player2Inventory.Remove(item);
                    }
                }

                break;
            case ItemType.Key:
                if ((player2InventoryRef.ContainsKey(item) && player2KeyItems.Contains(item)))
                {
                    player2InventoryRef[item] -= amount;

                    player2InventoryRef[item] = Mathf.Clamp(player2InventoryRef[item], 0, 1);

                    if (player2InventoryRef[item] <= 0)
                    {
                        player2KeyItems.Remove(item);
                    }
                }

                break;
            default:
                return;
            
                

        }
        UpdateInventorySlots();
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
    
    public void SwapKeyItem(InputAction.CallbackContext context)
    {
        
        if(player2KeyItems.Count == 0)
            return;
       
        currentSelectedKeyItemIndex += 1;
        
        if (currentSelectedKeyItemIndex >= player2KeyItems.Count)
        {
            currentSelectedKeyItemIndex = 0;
        }

        LoadKeyItem(); 
    }
    
    void LoadKeyItem()
    {
        currentSelectedKeyItem = player2KeyItems[currentSelectedKeyItemIndex];
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