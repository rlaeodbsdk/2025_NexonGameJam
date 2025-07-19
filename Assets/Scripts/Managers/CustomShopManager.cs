using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CustomShopManager : MonoBehaviour
{
    [Header("아이템상점 버튼")]
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

    [Header("상점 스크롤뷰")]
    public GameObject shopStage;
    public GameObject shopPanel;
    public GameObject leftShopCharacter;
    public GameObject rightShopCharacter;
    public GameObject upgradeScroll;
    public GameObject upgradeScrollContents;
    public GameObject foodScroll;
    public GameObject foodScrollContents;
    



    [Header("아이템 데이터")]
    public ItemSO[] itemData;
    public ItemSlot itemSlot;
    private Dictionary<ItemSO, int> playerInventory = new Dictionary<ItemSO, int>();
    private Dictionary<ItemSO, ItemSlot> itemSlotDict = new Dictionary<ItemSO, ItemSlot>();
    public TableManager tableManager;

    [Header("음식 데이터")]
    public FoodSO[] foodData;
    public FoodSlot foodSlot;
    private Dictionary<FoodSO, FoodSlot> foodSlotDict = new Dictionary<FoodSO, FoodSlot>();
    public static CustomShopManager instance;
    public List<FoodSO> foodList = new List<FoodSO>();

    public StoryManager stories;
    
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {

        stories = FindFirstObjectByType<StoryManager>();
        if(upgradeBtn != null && foodBtn !=null)
        {
            upgradeBtn.onClick.AddListener(ShowUpgradeMenu);
            foodBtn.onClick.AddListener(ShowFoodMenu);
            exitBtn.onClick.AddListener(CloseShop);
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

                // 슬롯 비활성화도 같이 진행!
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

        
        StartCoroutine(stories.getNextStory().TypingCoroutine());

    }

    public void BuyItem(ItemSO item)
    {
        if (Managers.Game.playerTotalMoney >= item.itemPrice) // 만약 구매 가능하다면
        {


            if (!playerInventory.ContainsKey(item))
                playerInventory[item] = item.instantAmount;
            if (playerInventory[item] < item.maxAmount)
                playerInventory[item] += 1;
            int curLevel = playerInventory[item];

            

            if (item.itemName == "식재료 가격 감소")
            {
                Managers.Game.ApplyIngredientDiscount(playerInventory[item]);
                if (curLevel == 2) item.itemPrice = 300;
                else if (curLevel == 3) item.itemPrice = 500;

            }

            if (item.itemName == "진상 등장 확률 감소")
            {
                Managers.Game.ApplyVillainRate(playerInventory[item]);
                if (curLevel == 1) item.itemPrice = 300;
                else if (curLevel == 2) item.itemPrice = 500;

            }

            if (item.itemName == "하루 시간 증가")
            {
                Managers.Game.ApplyDaytimeAddition(playerInventory[item]);
                if (curLevel == 1) item.itemPrice = 400;
                else if (curLevel == 2) item.itemPrice = 600;

            }

            if (item.type == ItemSO.itemType.Table)
            {
                tableManager.AddTable();
                if (curLevel == 1) item.itemPrice = 250;
                else if (curLevel == 2) item.itemPrice = 400;
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
        else // 구매 실패
        {
            Managers.Sound.Play("SFX/purchaseFailed1");
        }
    }

    public void BuyFood(FoodSO food)
    {
        if (Managers.Game.playerTotalMoney >= food.foodUnlockPrice)
        {
            if (foodSlotDict.ContainsKey(food))
                foodSlotDict[food].SetInteractable(false);

            if (!foodList.Contains(food))
                foodList.Add(food);

            Managers.Game.playerTotalMoney -= food.foodUnlockPrice;
            Managers.Sound.Play("SFX/purchase1");
        }
        else // 구매실패
        {
            Managers.Sound.Play("SFX/purchaseFailed1");
        }
    }

    public void openShop()
    {

        shopStage.SetActive(true);
    }
}
