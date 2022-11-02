using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;
using TMPro;
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }

    public GameObject dialoguePanel;
    
    //[System.NonSerialized]
    public bool isDialoguePlaying;

    [SerializeField] private TextMeshProUGUI dialogueText;

    private Story currentStory;

    public Player1DialogueManager p1;
    public Player2DialogueManager p2;
    
    
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

        p1 = FindObjectOfType<Player1DialogueManager>();
        p2 = FindObjectOfType<Player2DialogueManager>();
    }

    void Start()
    {
        isDialoguePlaying = false;
        
        dialoguePanel.SetActive(false);
        
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
