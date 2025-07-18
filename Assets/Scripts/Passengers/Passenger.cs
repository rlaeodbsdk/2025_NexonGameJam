using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Passenger : MonoBehaviour
{
    private List<FoodSO> foodList;
    public FoodSO selectedFood;
    public Table currentTable;
    private TableManager TableManager;

    public GameObject orderPanel;
    public Image orderedImage;

    private void OnEnable()
    {
        TableManager = FindFirstObjectByType<TableManager>();
        foodList = CustomShopManager.instance.foodList;
        StartCoroutine(Order());
    }
    public void Visit(Table table)
    {
        currentTable = table;
     
    }
    IEnumerator Order()
    {
        yield return new WaitForSeconds(2f);
        
        if (foodList == null || foodList.Count == 0) yield break;
        orderPanel.SetActive(true);
        int idx = Random.Range(0, foodList.Count);
        selectedFood = foodList[idx];
        orderedImage.sprite = selectedFood.foodImage;
        Debug.Log($"º’¥‘¿Ã {selectedFood.foodName} ¡÷πÆ«‘!");
        yield break;
    }

    // ≈¿Â
    public void Exit()
    {
        selectedFood = null;
       
        gameObject.SetActive(false);
    }
    public void SetFoodList(List<FoodSO> foods)
    {
        foodList = foods;
    }

}
