using UnityEngine;
using UnityEngine.UI;
using Characters;


public enum ItemType
{
    Regular,
    Gold,
    Potion,
    Mana,
    Map,
    BossKey
}

[CreateAssetMenu(fileName = "items1", menuName = "Data/Items", order = 0)]
public class ItemClass : ScriptableObject
{
        
    public int rateOfAppearing;
    public GameObject item;
    public ItemType type;
    public string description;
    public Sprite image;
    public int maxAmount;
    public int minAmount;
    public int HealAmount;
    public StatBoost Boost;
    public bool canUseItem = true;

    public void UseItem()
    {
        switch (type)
        {
            case ItemType.Potion:
                
                break;
            
            case ItemType.Mana:
                break;
        }
    }


}