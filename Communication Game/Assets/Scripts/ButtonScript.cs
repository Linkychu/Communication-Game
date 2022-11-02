using System;
using System.Linq;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public enum user
    {
        player1,
        player2,
        None
    }
    enum Player
    {
        none,
        Player1,
        Player2
    };
    public GameObject informationScreen;
    private TextMeshProUGUI text;
    public int id;
    private bool isPlayer1;
    private bool isPlayer2;
    private ItemClass item;
    public EventSystem eventSystem;

    public GameObject UseItemFirstSelected, GiveItemFirstSelected, DiscardItemFirstSelected;        
    public GameObject useItemCanvas;
    public GameObject selectedImage;

    [Header("Yes Or No Box")] public GameObject ConfirmationBox, ConfirmationText, NoBox;
    private TextMeshProUGUI confirmationText;
    private Player MainPlayer;
    private Player OtherPlayer;
    public user User;
    private Button button;
    private int index;

    [Header("Input How Much Box")] 
    public GameObject inputBox;
    private int amountInputted;
    public TextMeshProUGUI inputText;
    public GameObject increaseButton;
    private int boxId;

    public GlobalInventoryManager.InputDisplay OnInputDisplay;

    private bool open;
    
    
    private void Start()
    {
        amountInputted = 1;
        inputText.text = amountInputted.ToString();
        boxId = 0;
        button = GetComponent<Button>();
        text = informationScreen.GetComponentInChildren<TextMeshProUGUI>();
        
        confirmationText = ConfirmationText.GetComponent<TextMeshProUGUI>();
        index = 0;
        switch (User)
        {
            case user.player1:
                isPlayer1 = true;
                isPlayer2 = false;
                MainPlayer = Player.Player1;
                OtherPlayer = Player.Player2;
                if(GlobalInventoryManager.instance.p1.player1Inventory.Count < id || GlobalInventoryManager.instance.p1.player1Inventory.Count == 0)
                    return;
                if(GlobalInventoryManager.instance.p1.player1Inventory.ElementAtOrDefault(id) == null)
                {
                    Debug.Log("Fail");
                    return;
                }

                Debug.Log("P1 " + id);
                item = GlobalInventoryManager.instance.p1.player1Inventory[id];
               
                break;
            case user.player2:
                isPlayer1 = false;
                isPlayer2 = true;
                MainPlayer = Player.Player2;
                OtherPlayer = Player.Player1;
                if(GlobalInventoryManager.instance.p2.player2Inventory.Count < id || GlobalInventoryManager.instance.p2.player2Inventory.Count == 0)
                    return;
                Debug.Log("P2" + id);
                if(GlobalInventoryManager.instance.p2.player2Inventory.ElementAtOrDefault(id) == null)
                {
                    Debug.Log("Fail");
                    return;
                }
                item = GlobalInventoryManager.instance.p2.player2Inventory[id];
                break;
            default:
                isPlayer1 = false;
                isPlayer2 = false;
                MainPlayer = Player.none;
                OtherPlayer = Player.none;
                break;
        }
    }

    private void OnInputDisplayItem()
    {
        if (!open)
        {
            button.interactable = false;
        }
        
        
    }

    public void OnHoverEnter()
    {
        
        
        if (isPlayer1)
        {
            if (item != null)
            {
                informationScreen.SetActive(true);
                text.text = item.description;
            }

            else
            {
                text.text = String.Empty;
                informationScreen.SetActive(false);
            }
        }

        else if(isPlayer2)
        {

            if (item != null)
            {
                informationScreen.SetActive(true);
                text.text = item.description;
                
            }

            else
            {
                informationScreen.SetActive(false);
            }

        }
    }

    public void OnclickEvent()
    {
        button.interactable = false;
        useItemCanvas.SetActive(true);

        if (isPlayer2)
        {
            eventSystem.SetSelectedGameObject(UseItemFirstSelected);
        }
        
        /*if (!item.canUseItem)
            return;    */
    }

    public void OnHoverExit()
    {
        informationScreen.SetActive(false);
        
    }

    public void OnSelect()
    {
        if (isPlayer2)
        {
            selectedImage.SetActive(true);
        }
    }
    
    

    private void OnDisable()
    {
        useItemCanvas.SetActive(false);
        if (isPlayer2)
        {
            selectedImage.SetActive(false);
        }
        OnHoverExit();
        Cancel();
    }

    public void UseItemButton()
    {
        
        ConfirmationBox.SetActive(true);
        
        if (isPlayer2)
        {
            eventSystem.SetSelectedGameObject(NoBox);
        }

        confirmationText.text = $"Are You Sure You Want To Use this Item On {OtherPlayer}";
        index = 1;
    }

    public void GiveItemButton()
    {
        ConfirmationBox.SetActive(true);
        
        if (isPlayer2)
        {
            eventSystem.SetSelectedGameObject(NoBox);
        }

        confirmationText.text = $"Are You Sure You Want To Give this Item To {OtherPlayer}";
        index = 2;
    }

    public void DiscardItemButton()
    {
        ConfirmationBox.SetActive(true);
        
        if (isPlayer2)
        {
            eventSystem.SetSelectedGameObject(NoBox);
        }
        confirmationText.text = $"Are You Sure You Want To Discard this Item";
        index = 3;
    }

    void DisplayInputAmountBox()
    {
        ConfirmationBox.SetActive(false);
        inputBox.SetActive(true);
        eventSystem.SetSelectedGameObject(increaseButton);
        open = true;
        OnInputDisplay += OnInputDisplayItem;
        OnInputDisplay();
    }

    public void InputItem()
    {
        if(amountInputted <= 0)
            return;
        switch (boxId)
        {
            case 1:
                GiveItem(amountInputted);
                break;
            case 2:
                DiscardItem(amountInputted);
                break;
        }
    }

    public void UpdateItemCount(int Index)
    {
        switch (Index)
        {
            case 1:
                amountInputted += 1;
                break;
            case 2:
                amountInputted -= 1;
                break;
                
        }
        
        
        inputText.text = amountInputted.ToString();
    }
    public void Confirmation()
    {
        switch (index)
        {
            case 1:
                ConfirmationBox.SetActive(false);
                useItemCanvas.SetActive(false);
                button.interactable = true;
                UseItem();
                break;
            case 2:
                ConfirmationBox.SetActive(false);
                useItemCanvas.SetActive(false);
                button.interactable = true;
                boxId = 1;
                DisplayInputAmountBox();
                break;
            case 3:
                ConfirmationBox.SetActive(false);
                useItemCanvas.SetActive(false);
                button.interactable = true;
                boxId = 2;
                DisplayInputAmountBox();
                break;
            default:
                Cancel();
                break;
        }
    }
    
     void UseItem()
     {
         int i = 0;
         switch (OtherPlayer)
         {
             case Player.Player1:
                 item.UseItem(null, PlayerStateManager.instance.players[1]);
                 GlobalInventoryManager.instance.p1.CloseInventory();
                 GlobalInventoryManager.instance.p2.SubtractItem(item, 1);
                 break;
             case Player.Player2:
                 item.UseItem(null, PlayerStateManager.instance.players[0]);
                 GlobalInventoryManager.instance.p2.CloseInventory();
                 GlobalInventoryManager.instance.p1.SubtractItem(item, 1);
                 break;
         }
         
         
         
     }

     void GiveItem(int amount)
    {
        switch (MainPlayer)
        {
            case Player.Player1:
                if (GlobalInventoryManager.instance.p1.HasEnoughItem(item, amount))
                {
                    GlobalInventoryManager.instance.SendItem(item, amount, PlayerState.Player2);
                    GlobalInventoryManager.instance.p1.CloseInventory();
                   
                }
                break;
            case Player.Player2:
                if (GlobalInventoryManager.instance.p2.HasEnoughItem(item, amount))
                {
                    GlobalInventoryManager.instance.SendItem(item, amount, PlayerState.Player1);
                    GlobalInventoryManager.instance.p2.CloseInventory();
                }
                
                break;
        }
       
    }

     void DiscardItem(int amount)
    {
        switch (MainPlayer)
        {
            case Player.Player1:
                GlobalInventoryManager.instance.p1.SubtractItem(item, amount);
                GlobalInventoryManager.instance.p1.CloseInventory();
                GlobalInventoryManager.instance.DropItem(item, amount, PlayerState.Player1);
                break;
            case Player.Player2:
                GlobalInventoryManager.instance.p2.SubtractItem(item, amount);
                GlobalInventoryManager.instance.p2.CloseInventory();
                GlobalInventoryManager.instance.DropItem(item, amount, PlayerState.Player2);
                break;
        }
    }

    public void Cancel()
    {
        useItemCanvas.SetActive(false);
        ConfirmationBox.SetActive(false);
        confirmationText.text = String.Empty;
        inputBox.SetActive(false);
        OnInputDisplay -= OnInputDisplayItem;
        button.interactable = true;
        open = false;

    }
    
    
}