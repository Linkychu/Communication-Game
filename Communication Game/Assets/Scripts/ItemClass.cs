using UnityEngine;
using UnityEngine.UI;
using Characters;


public enum ItemType
{
   Normal,
   Key
}

[CreateAssetMenu(fileName = "items1", menuName = "Data/Items", order = 0)]
public class ItemClass : ScriptableObject
{
    public string name;
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

    public void UseItem(CharacterClass Class, PlayerClass player)
    {
        switch (type)
        {
            case ItemType.Normal:
                item.GetComponent<ItemBase>().UseItem(Class, player);
                break;
            case ItemType.Key:
                item.GetComponent<KeyItemBase>().UseItem(Class, player);
                break;
        }
    }


}