using System.Collections.Generic;
using UnityEngine;

public class ShopDataManager : MonoBehaviour
{
    public static ShopDataManager instance;

    // ������ ���� (ItemSO.name, level)
    public Dictionary<string, int> itemLevels = new Dictionary<string, int>();

    // �رݵ� ���� ����Ʈ (FoodSO.name)
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

    // ����� �Լ� (��: ���߿� ���� ���� ��� ���̱� ����)
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
