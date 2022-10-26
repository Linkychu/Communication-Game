using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public List<ItemClass> NormalItems = new List<ItemClass>();

    public List<ItemClass> KeyItems = new List<ItemClass>();

    
    private GameObject[] chests;

    private int keyItemIndex = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Generate()
    {
        chests = GameObject.FindGameObjectsWithTag("Chest");

        foreach (var chest in chests)
        {
            chest.transform.parent = transform;
        }
        GenerateKeyItems();
    }
    
     void GenerateKeyItems()
    {
        int failure = 0;
        int i = 0;
        List<GameObject> KeyChests = new List<GameObject>();
        while ((KeyChests.Count < KeyItems.Count) && failure < 5000)
        {
            var index = SeededRandom.Range(0, chests.Length);
            if (keyItemIndex == index)
            {
                index = SeededRandom.Range(0, chests.Length);
                failure++;
                
            }
            else
            {
                var Chest = chests[index].GetComponent<OpenItem>();
                Chest.item = KeyItems[i].item;
                Chest.type = ChestType.KeyChest;
                keyItemIndex = index;
                KeyChests.Add(chests[index]);
                Chest.itemClass = KeyItems[i];
                Chest.itemGenerated = true;
                i++;
            }
        }
        
        GenerateNormalItems();
    }

    void GenerateNormalItems()
    {
        GameObject chest;
        List<OpenItem> items = new List<OpenItem>();
        foreach (var Chest in chests)
        {
            var openItem = Chest.GetComponent<OpenItem>();
            if (openItem.type == ChestType.normalChest)
            {
                items.Add(openItem);
            }
        }

        foreach (var item in items)
        {
            int randomIndex = SeededRandom.Range(0, NormalItems.Count);
            int randomRange = SeededRandom.Range(0, 100);
            if (randomRange < NormalItems[randomIndex].rateOfAppearing)
            {
                item.item = NormalItems[randomIndex].item;
                item.itemClass = NormalItems[randomIndex];
                item.itemGenerated = true;
            }

            else
            {
                Destroy(item.gameObject);
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
