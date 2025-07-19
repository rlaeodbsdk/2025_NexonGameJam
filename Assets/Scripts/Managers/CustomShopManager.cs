using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CustomShopManager : MonoBehaviour
{
    [Header("�����ۻ��� ��ư")]
    public Image upgradeBtnImage;
    public Image foodBtnImage;
    public Button upgradeBtn;
    public Button foodBtn;
    public Button exitBtn;
    public Sprite upgradeBtnActiveSprite;
    public Sprite upgradeBtnInactiveSprite;
    public Sprite foodBtnActiveSprite;
    public Sprite foodBtnInactiveSprite;
    public RectTransform upgradeBtnRect;
    public RectTransform foodBtnRect;

    [Header("���� ��ũ�Ѻ�")]
    public GameObject shopStage;
    public GameObject shopPanel;
    public GameObject leftShopCharacter;
    public GameObject rightShopCharacter;
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

    public event Action OnShopClosed;

    private void Awake()
    {
 
        InitShopDataManager();

        instance = this;
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // �̹� �ٸ� �ν��Ͻ��� ������ �ڽ� �ı�
            return;
        }
    }

    private void Start()
    {
        Time.timeScale = 0;

        if (upgradeBtn != null && foodBtn != null && exitBtn != null)
        {
            upgradeBtn.onClick.AddListener(ShowUpgradeMenu);
            foodBtn.onClick.AddListener(ShowFoodMenu);
            exitBtn.onClick.AddListener(CloseShop);
        }

        // ������ ���� �ʱ�ȭ
        for (int i = 0; i < itemData.Length; i++)
        {
            int level = ShopDataManager.instance.GetItemLevel(itemData[i].itemName);

            var slot = Instantiate(itemSlot, upgradeScrollContents.transform);
            slot.SetData(itemData[i], level);
            itemSlotDict[itemData[i]] = slot;
            playerInventory[itemData[i]] = level;
            if (level >= itemData[i].maxAmount)
                slot.SetInteractable(false);
            else
                slot.SetInteractable(true);
        }

        // ���� ���� �ʱ�ȭ
        for (int i = 0; i < foodData.Length; i++)
        {
            var slot = Instantiate(foodSlot, foodScrollContents.transform);
            slot.SetData(foodData[i]);
            foodSlotDict[foodData[i]] = slot;

            if (ShopDataManager.instance.IsFoodUnlocked(foodData[i].name))
            {
                foodList.Add(foodData[i]);
                slot.SetInteractable(false);
            }
            

        }
        int tableCount = ShopDataManager.instance.GetItemLevel("���̺�");
        for (int i = 0; i < tableCount; i++)
        {
            tableManager.AddTable();
        }
        // �رݵ� ���� �ٽ� ����
        foodList.Clear();
        foreach (var food in foodData)
        {
            if (food.isActive)
            {
                foodList.Add(food);

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
        upgradeBtnImage.sprite = upgradeBtnActiveSprite;
        foodBtnImage.sprite = foodBtnInactiveSprite;
        upgradeBtnRect.SetAsLastSibling();
        foodBtnRect.SetAsFirstSibling();
    }

    public void ShowFoodMenu()
    {
        upgradeScroll?.SetActive(false);
        foodScroll?.SetActive(true);
        upgradeBtnImage.sprite = upgradeBtnInactiveSprite;
        foodBtnImage.sprite = foodBtnActiveSprite;
        foodBtnRect.SetAsLastSibling();
        upgradeBtnRect.SetAsFirstSibling();
    }

    public void CloseShop()
    {
        shopPanel.gameObject.GetComponent<DOTweenAnimation>().DOPlayBackwards();
        leftShopCharacter.gameObject.GetComponent<DOTweenAnimation>().DOPlayBackwards();
        rightShopCharacter.gameObject.GetComponent<DOTweenAnimation>().DOPlayBackwards();

        OnShopClosed?.Invoke();
        Time.timeScale = 1;

    }
    public void BuyItem(ItemSO item)
    {
        if (Managers.Game.playerTotalMoney >= item.itemPrice) // ���� ���� �����ϴٸ�
        {
            if (!playerInventory.ContainsKey(item))
                playerInventory[item] = item.instantAmount;
            if (playerInventory[item] < item.maxAmount)
                playerInventory[item] += 1;
            int curLevel = playerInventory[item];

            ShopDataManager.instance.SaveItemLevel(item.itemName, curLevel);

            if (item.itemName == "����� ���� ����")
            {
                Managers.Game.ApplyIngredientDiscount(playerInventory[item]);
                if (curLevel == 2) item.itemPrice = 300;
                else if (curLevel == 3) item.itemPrice = 500;
                ShopDataManager.instance.SaveItemLevel(item.itemName, playerInventory[item]);
            }

            if (item.itemName == "���� ���� Ȯ�� ����")
            {
                Managers.Game.ApplyVillainRate(playerInventory[item]);
                if (curLevel == 1) item.itemPrice = 300;
                else if (curLevel == 2) item.itemPrice = 500;
                ShopDataManager.instance.SaveItemLevel(item.itemName, playerInventory[item]);
            }

            if (item.itemName == "�Ϸ� �ð� ����")
            {
                Managers.Game.ApplyDaytimeAddition(playerInventory[item]);
                if (curLevel == 1) item.itemPrice = 400;
                else if (curLevel == 2) item.itemPrice = 600;
                ShopDataManager.instance.SaveItemLevel(item.itemName, playerInventory[item]);
            }

            if (item.type == ItemSO.itemType.Table)
            {
                tableManager.AddTable();
                if (curLevel == 1) item.itemPrice = 250;
                else if (curLevel == 2) item.itemPrice = 400;
              
                ShopDataManager.instance.SaveItemLevel(item.itemName, playerInventory[item]);
            }

            if (itemSlotDict.ContainsKey(item))
            {
                itemSlotDict[item].SetData(item, playerInventory[item]);
                if (playerInventory[item] >= item.maxAmount)
                    itemSlotDict[item].SetInteractable(false);
                else
                    itemSlotDict[item].SetInteractable(true);
            }

   
            Managers.Game.playerTotalMoney -= item.itemPrice;
            Managers.Sound.Play("SFX/purchase1");
        }
        else // ���� ����
        {
            Managers.Sound.Play("SFX/purchaseFailed1");
        }
    }

    public void BuyFood(FoodSO food)
    {
        if (Managers.Game.playerTotalMoney >= food.foodUnlockPrice)
        {
            ShopDataManager.instance.UnlockFood(food.name); // ����!

            if (foodSlotDict.ContainsKey(food))
                foodSlotDict[food].SetInteractable(false);

            if (!foodList.Contains(food))
                foodList.Add(food);

            Managers.Game.playerTotalMoney -= food.foodUnlockPrice;
            Managers.Sound.Play("SFX/purchase1");
        }
        else // ���Ž���
        {
            Managers.Sound.Play("SFX/purchaseFailed1");
        }
    }

    void InitShopDataManager()
    {
        if (ShopDataManager.instance == null)
        {
            GameObject obj = new GameObject("ShopDataManager");
            obj.AddComponent<ShopDataManager>();
        }
        
    }

    public void openShop()
    {

        shopStage.SetActive(true);
    }
}
