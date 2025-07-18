using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CustomShopManager : MonoBehaviour
{
    [Header("�����ۻ��� ��ư")]
    public Button upgradeBtn;
    public Button foodBtn;

    [Header("���� ��ũ�Ѻ�")]
    public GameObject upgradeScroll;
    public GameObject upgradeScrollContents;
    public GameObject foodScroll;
    public GameObject foodScrollContents;


    [Header("������ ������")]
    public ItemSO[] itemData;
    public ItemSlot itemSlot;
    private Dictionary<ItemSO, int> playerInventory = new Dictionary<ItemSO, int>();
    private Dictionary<ItemSO, ItemSlot> itemSlotDict = new Dictionary<ItemSO, ItemSlot>();
    public TableManager tableManager;

    [Header("���� ������")]
    public FoodSO[] foodData;
    public FoodSlot foodSlot;
    private Dictionary<FoodSO, FoodSlot> foodSlotDict = new Dictionary<FoodSO, FoodSlot>();
    public static CustomShopManager instance;
    public List<FoodSO> foodList = new List<FoodSO>();
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if(upgradeBtn != null && foodBtn !=null)
        {
            upgradeBtn.onClick.AddListener(ShowUpgradeMenu);
            foodBtn.onClick.AddListener(ShowFoodMenu);
        }
        for (int i = 0; i < itemData.Length; i++)
        {
            var slot = Instantiate(itemSlot, upgradeScrollContents.transform);
            slot.SetData(itemData[i], itemData[i].instantAmount);
            itemSlotDict[itemData[i]] = slot;

        }
        for (int i = 0; i < foodData.Length; i++)
        {
            var slot = Instantiate(foodSlot, foodScrollContents.transform);
            slot.SetData(foodData[i]);
            foodSlotDict[foodData[i]] = slot;
        }

        foodList.Clear();
        foreach (var food in foodData)
        {
            if (food.isActive)
            {
                foodList.Add(food);

                // ���� ��Ȱ��ȭ�� ���� ����!
                if (foodSlotDict.ContainsKey(food))
                {
                    foodSlotDict[food].SetInteractable(false);
                }
            }
        }
    }

    public void ShowUpgradeMenu()
    {
        upgradeScroll?.SetActive(true);
        foodScroll?.SetActive(false);
    }

    public void ShowFoodMenu()
    {
        upgradeScroll?.SetActive(false);
        foodScroll?.SetActive(true);
    }

    public void BuyItem(ItemSO item)
    {
        if (!playerInventory.ContainsKey(item))
            playerInventory[item] = item.instantAmount;

        if(playerInventory[item] < item.maxAmount)
            playerInventory[item] += 1;

        
        if (itemSlotDict.ContainsKey(item))
        {
            itemSlotDict[item].SetData(item, playerInventory[item]);
            if (playerInventory[item] >= item.maxAmount)
                itemSlotDict[item].SetInteractable(false);
            else
                itemSlotDict[item].SetInteractable(true);
        }

        if (item.type == ItemSO.itemType.Table)
        {
            tableManager.AddTable();
        }
    }

    public void BuyFood(FoodSO food)
    {
        if (foodSlotDict.ContainsKey(food))
            foodSlotDict[food].SetInteractable(false);

        if (!foodList.Contains(food))
            foodList.Add(food);
    }
}
