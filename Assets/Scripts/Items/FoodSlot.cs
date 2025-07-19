using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class FoodSlot : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI foodName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI price;
    public bool isActive;
    public TextMeshProUGUI foodNodeInfo;

    private FoodSO foodData;

    public Button slotButton;
    public void SetData(FoodSO foodData)
    {
        this.foodData = foodData;
        SetValue();
        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(() => CustomShopManager.instance.BuyFood(foodData));
    }
    public void SetValue()
    {
        this.isActive = foodData.isActive;
        image.sprite = foodData.foodImage;
        foodName.SetText(foodData.foodName);
        description.SetText(foodData.foodInfo);
        foodNodeInfo.SetText(foodData.foodNodeInfo);
        if (foodData.foodUnlockPrice == 0)
        {
            price.SetText("판매가: " + foodData.foodPrice + "냥");
        }
        else
        {
            price.SetText("판매가: " + foodData.foodPrice + "냥 / 해금: " + foodData.foodUnlockPrice + "냥");
        }
    }

    public void SetInteractable(bool isActive)
    {
        slotButton.interactable = isActive;
    }
}
