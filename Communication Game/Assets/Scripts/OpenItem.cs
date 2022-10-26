using System;
using UnityEngine;
using System.Collections;
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
        GlobalInventoryManager.instance.p1.playerInput.Inventory.Interact.performed += ActionPerformed;
        GlobalInventoryManager.instance.p2.playerInput.Inventory.Interact.performed += ActionPerformed;
    }

    void ActionPerformed(InputAction.CallbackContext context)
    {
        if(!isPlayerDetected)
            return;
        if(input)
            return;
        StartCoroutine(OpenChest());
    }
    

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerDetected = other.transform.parent.TryGetComponent(out PlayerManager playerManager);
            manager = playerManager;
            switch (playerManager.playerState)
            {
                case PlayerState.Player1:
                    playerId = 1;
                    break;
                case PlayerState.Player2:
                    playerId = 2;
                    break;
                default:
                    playerId = 0;
                    break;
            }
        }
    }


    IEnumerator OpenChest()
    {
        input = true;
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
                    GlobalInventoryManager.instance.p1.AddItem(itemClass, amount);
                    break;
                case 2:
                    manager.GetComponentInChildren<PlayerTwoMovement>().canMove = true;   
                    GlobalInventoryManager.instance.p2.AddItem(itemClass, amount);
                    break;
            }

            /*
            if (itemClass.type == ItemType.Map)
            {
                GameManager.instance.CheckForMap(true);
            }*/



        }
    }
}