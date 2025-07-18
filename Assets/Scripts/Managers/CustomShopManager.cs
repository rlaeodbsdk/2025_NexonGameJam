using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CustomShopManager : MonoBehaviour
{
    [Header("아이템상점 버튼")]
    public Button upgradeBtn;
    public Button foodBtn;

    [Header("상점 스크롤뷰")]
    public GameObject upgradeScroll;
    public GameObject upgradeScrollContents;
    public GameObject foodScroll;
    public GameObject foodScrollContents;


    [Header("아이템 데이터")]
    public ItemSO[] itemData;
    public ItemSlot itemSlot;
    private Dictionary<ItemSO, int> playerInventory = new Dictionary<ItemSO, int>();
    private Dictionary<ItemSO, ItemSlot> itemSlotDict = new Dictionary<ItemSO, ItemSlot>();

    [Header("음식 데이터")]
    public FoodSO[] foodData;
    public FoodSlot foodSlot;

    public static CustomShopManager instance;
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
            slot.SetData(itemData[i], 0);
            itemSlotDict[itemData[i]] = slot;

        }
        for (int i = 0; i < foodData.Length; i++)
        {
            var slot = Instantiate(foodSlot, foodScrollContents.transform);
            slot.SetData(foodData[i]);
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
            playerInventory[item] = 0;
        playerInventory[item] += 1;

        
        if (itemSlotDict.ContainsKey(item))
            itemSlotDict[item].SetData(item, playerInventory[item]);
    }

    public void BuyFood(FoodSO food)
    {
        
    }
}
