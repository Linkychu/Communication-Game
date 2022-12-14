using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.InputSystem;


public enum ChestType
{
    normalChest,
    KeyChest
}

public class OpenItem : MonoBehaviour
{
    private bool isOpened;
    
    private MeshFilter _filter;


    [HideInInspector]
    public GameObject item;


   
    public bool itemGenerated;


    public ChestType type;


    
    public ItemClass itemClass;

    [SerializeField] private TextAsset dialogueText;
    [SerializeField] private TextAsset tooMuchText;

    private int playerId;

    private bool isPlayerDetected;

    private PlayerManager manager;

    private bool input;
    
    /*public int itemRate = 80;
    private int itemChance => RNG.RngCallRange(0, 100);*/

    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        isOpened = false;
        input = false;
    }

    public void ActionPerformed(int id, PlayerManager Manager)
    {
        if(input)
            return;
        playerId = id;
        manager = Manager;
        StartCoroutine(OpenChest());
    }
    
    


    public IEnumerator OpenChest()
    {
        input = true;
        item = itemClass.item;
        if (itemGenerated)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            switch (playerId)
            {
                case 1:
                    manager.GetComponentInChildren<PlayerOneMovement>().canMove = false;       
                    break;
                case 2:
                    manager.GetComponentInChildren<PlayerTwoMovement>().canMove = false;       
                    break;
                default:
                    break;
            }

            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);

            isOpened = true;

            GameObject ins = Instantiate(item,
                new Vector3(transform.position.x, item.transform.position.y, transform.position.z),
                item.transform.rotation);

            var position = ins.transform.position;
            position = new Vector3(position.x, position.y + 2, position.z);
            ins.transform.position = position;

            int amount = 1;
            if(type == ChestType.normalChest)
            {
                int RandomAmount = SeededRandom.Range(itemClass.minAmount, itemClass.maxAmount);
                int randomRange = SeededRandom.Range(0, 100);
            
                int ratio = Extensions.GreatestCommonDenominator(itemClass.minAmount, itemClass.maxAmount);
                int ratioA = Mathf.FloorToInt(itemClass.minAmount / ratio);
                int ratioB = Mathf.FloorToInt(itemClass.maxAmount / ratio);
                RandomAmount = randomRange * ratioA < RandomAmount * ratioB ? RandomAmount : itemClass.minAmount;

                amount = RandomAmount;
                amount = Mathf.Clamp(amount, 1, itemClass.maxAmount);
                
                Debug.Log(amount);
            }
            
            switch (playerId)
            {
                case 1:
                    DialogueManager.instance.p1.DisplayNewItem(dialogueText, itemClass.name, amount);
                    while (DialogueManager.instance.p1.isDialoguePlaying)
                    {
                        yield return null;
                        //yield return new WaitUntil(() => DialogueManager.instance.isDialoguePlaying = false);
                    }

                    break;
                case 2:
                    DialogueManager.instance.p2.DisplayNewItem(dialogueText, itemClass.name, amount);
                    while (DialogueManager.instance.p2.isDialoguePlaying)
                    {
                        yield return null;
                        //yield return new WaitUntil(() => DialogueManager.instance.isDialoguePlaying = false);
                    }
                    break;
                default:
                    break;
            }
      
            Destroy(ins);

           
           
            

            switch (playerId)
            {
                case 1:
                    manager.GetComponentInChildren<PlayerOneMovement>().canMove = true;
                    switch (itemClass.type)
                    {
                        case ItemType.Key:
                            if (hasEnoughKeyItem(itemClass, PlayerState.Player1))
                            {
                                GlobalInventoryManager.instance.p1.AddItem(itemClass, amount);
                            }

                            else
                            {
                                DialogueManager.instance.p1.TooMuchItem(tooMuchText);
                                transform.GetChild(0).gameObject.SetActive(true);
                                transform.GetChild(1).gameObject.SetActive(false);
                                
                            }
                            break;
                        default:
                            GlobalInventoryManager.instance.p1.AddItem(itemClass, amount);
                            break;
                    }
                   
                    break;
                case 2:
                    manager.GetComponentInChildren<PlayerTwoMovement>().canMove = true;   
                    switch (itemClass.type)
                    {
                        case ItemType.Key:
                            if (hasEnoughKeyItem(itemClass, PlayerState.Player2))
                            {
                                GlobalInventoryManager.instance.p2.AddItem(itemClass, amount);
                            }

                            else
                            {
                                DialogueManager.instance.p2.TooMuchItem(tooMuchText);
                                transform.GetChild(0).gameObject.SetActive(true);
                                transform.GetChild(1).gameObject.SetActive(false);
                            }
                            break;
                        default:
                            GlobalInventoryManager.instance.p2.AddItem(itemClass, amount);
                            break;
                    }
                    
                    break;
            }

            /*
            if (itemClass.type == ItemType.Map)
            {
                GameManager.instance.CheckForMap(true);
            }*/

            

        }
        
        
    }

    public bool hasEnoughKeyItem(ItemClass x, PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Player1:
                if (GlobalInventoryManager.instance.p1.player1KeyItems.Any(items => items.name == x.name))
                {
                    return false;
                }

                else
                {
                    return true;
                }
                
                break;
            case PlayerState.Player2:
                if (GlobalInventoryManager.instance.p2.player2KeyItems.Any(items => items.name == x.name))
                {
                    return false;
                }

                else
                {
                    return true;
                }
                break;
        }

        return false;
    }
}