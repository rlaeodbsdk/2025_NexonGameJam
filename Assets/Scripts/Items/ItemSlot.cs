using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI description;
    public TextMeshProUGUI price;
    public TextMeshProUGUI amount;
    public bool isActive;
    public Button slotButton;

    private ItemSO itemData;

    public void SetData(ItemSO itemData, int amountValue)
    {   
        this.itemData = itemData;
        SetValue(amountValue);
        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(() => CustomShopManager.instance.BuyItem(itemData));
        
        
    }
    public void SetValue(int amountValue)
    {
         this.isActive = itemData.isActive;
        image.sprite = itemData.itemImage;
        itemName.SetText(itemData.itemName);
        description.SetText(itemData.itemInfo);
        if(itemData.itemUnlockPrice == 0)
        {
            price.SetText("판매가: " + itemData.itemPrice + "냥");
        }
        else
        {
            price.SetText("판매가: " + itemData.itemPrice + "냥 / 해금: " + itemData.itemUnlockPrice + "냥");
        }

            amount.SetText(amountValue + " 개");
        
    }
    public void SetInteractable(bool isActive)
    {
        slotButton.interactable = isActive;
    }
}
