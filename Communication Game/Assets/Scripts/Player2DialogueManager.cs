using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;
using UnityEngine.InputSystem;
using TMPro;

public class Player2DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;

    private Player2 player2Input;
    
    public bool isDialoguePlaying;

    [SerializeField] private TextMeshProUGUI dialogueText;

    private Story currentStory;

    private bool input2Pressed;


    private void Awake()
    {
        player2Input = new Player2();
        player2Input.UI.AnyKey.performed += OnButtonPressed;
    }

    // Start is called before the first frame update
    void Start()
    {
        isDialoguePlaying = false;
        
        dialoguePanel.SetActive(false);
        
        
           
    }

    private void OnEnable()
    {
        player2Input.UI.Enable();
        
    }

    private void OnDisable()
    {
        player2Input.UI.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    
    public void OnButtonPressed(InputAction.CallbackContext context)
    {
        if(!isDialoguePlaying)
            return;
        ContinueStory();
       
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
