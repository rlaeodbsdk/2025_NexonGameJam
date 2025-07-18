using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodData", menuName = "Food/Foods")]
public class FoodSO : ScriptableObject
{
    public bool isActive = false;
    public string foodName;
    public Sprite foodImage;
    public int foodPrice;
    public int foodUnlockPrice;

    [TextArea]
    public string foodInfo;
}
