using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance { get; set; }
    
    private void Awake()
    {
        instance = this;
    }
}