using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class FoodSlot : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI foodName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI price;
    public bool isActive;

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
        price.SetText("price: " + foodData.foodPrice);
    }

    public void SetInteractable(bool isActive)
    {
        slotButton.interactable = isActive;
    }
}
