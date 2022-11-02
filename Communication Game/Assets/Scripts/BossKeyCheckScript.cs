using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossKeyCheckScript : MonoBehaviour
{

    [HideInInspector]
    public bool hasOneKey;

    [HideInInspector]
    public bool hasTwoKey;

    [SerializeField] private TextAsset dialogue;

    public bool isDoorOpen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isDoorOpen)
            return;
        if (hasOneKey && hasTwoKey)

        {
            OpenDoor();

        }
        
    }

    private void LateUpdate()
    {
        if (hasOneKey || hasTwoKey)
        {
            hasOneKey = false;
            hasTwoKey = false;
        }
    }

    void OpenDoor()
    {
        DialogueManager.instance.p1.DisplayDialogue(dialogue);
        DialogueManager.instance.p2.DisplayDialogue(dialogue);
        ItemClass Class =
            GlobalInventoryManager.instance.p1.player1KeyItems.FirstOrDefault(items => items.name == "Key");
        if(Class != null)
        {
            GlobalInventoryManager.instance.p1.SubtractItem(Class, 1);
        }
        
        ItemClass Class2 =
            GlobalInventoryManager.instance.p2.player2KeyItems.FirstOrDefault(items => items.name == "Key");
        if(Class2 != null)
        {
            GlobalInventoryManager.instance.p2.SubtractItem(Class, 1);
        }
        gameObject.SetActive(false);
    }
}
