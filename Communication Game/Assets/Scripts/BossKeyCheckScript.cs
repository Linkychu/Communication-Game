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

   

    void OpenDoor()
    {
        DialogueManager.instance.p1.DisplayDialogue(dialogue);
        DialogueManager.instance.p2.DisplayDialogue(dialogue);
        hasOneKey = false;
        hasTwoKey = false;
        gameObject.SetActive(false);
        
    }
}
