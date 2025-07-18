using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "itemData", menuName = "item/items")]
public class ItemSO : ScriptableObject
{

    public enum itemType{
        Table,
    }
    public bool isActive;
    public itemType type;   
    public string itemName;
    public Sprite itemImage;
    public int instantAmount;
    public int itemPrice;
    public int popularity;
    public int maxAmount;
    [TextArea]
    public string itemInfo;
}
