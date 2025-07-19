using System.Collections.Generic;
using UnityEngine;

public class ShopDataManager : MonoBehaviour
{
    public static ShopDataManager instance;

    // 아이템 상태 (ItemSO.name, level)
    public Dictionary<string, int> itemLevels = new Dictionary<string, int>();

    // 해금된 음식 리스트 (FoodSO.name)
    public HashSet<string> unlockedFoods = new HashSet<string>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 저장용 함수 (예: 나중에 파일 저장 기능 붙이기 쉬움)
    public void SaveItemLevel(string itemName, int level)
    {
        itemLevels[itemName] = level;
    }

    public void UnlockFood(string foodName)
    {
        unlockedFoods.Add(foodName);
    }

    public int GetItemLevel(string itemName)
    {
        return itemLevels.ContainsKey(itemName) ? itemLevels[itemName] : 0;
    }

    public bool IsFoodUnlocked(string foodName)
    {
        return unlockedFoods.Contains(foodName);
    }
}
