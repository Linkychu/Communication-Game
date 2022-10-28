using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;
using TMPro;
using UnityEngine.InputSystem;

public class Player1DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;

    private Player1 player1Input;
    
    public bool isDialoguePlaying;

    [SerializeField] private TextMeshProUGUI dialogueText;

    private Story currentStory;

    private bool input1Pressed;
    // Start is called before the first frame update
    private void Awake()
    {
        player1Input = new Player1();
        player1Input.UI.AnyKey.performed += OnButtonPressed;
    }

    void Start()
    {
        isDialoguePlaying = false;
        
        dialoguePanel.SetActive(false);
        
        
           
    }

    private void OnEnable()
    {
        player1Input.UI.Enable();
        
    }

    public void OnButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Working");
        if(!isDialoguePlaying)
            return;
        ContinueStory();
        
    }
    private void OnDisable()
    {
        player1Input.UI.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
       

    }
    
    public void DisplayDialogue(TextAsset dialogue, int id)
    {
        
        currentStory = new Story(dialogue.text);
        isDialoguePlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
       
    }
    
    public void DisplayNewItem(TextAsset dialogue, string Name, int amount)
    {
        
        currentStory = new Story(dialogue.text);
        currentStory.variablesState["item"] = Name;
        currentStory.variablesState["amount"] = amount;
        isDialoguePlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
       
    }

    public void TradedItem(TextAsset tradedItem, string TradedPlayer, string itemName, int amount)
    {
        currentStory = new Story(tradedItem.text);
        currentStory.variablesState["TradedPlayer"] = TradedPlayer;
        currentStory.variablesState["item"] = itemName;
        currentStory.variablesState["amount"] = amount;
        isDialoguePlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }
    


    void ExitDialogueMode()
    {
        isDialoguePlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = String.Empty;
    }

    void ContinueStory()
    {
        
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }

        else
        {
            ExitDialogueMode();
        }
    }
  
}
