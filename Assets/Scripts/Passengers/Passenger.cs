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
    public Sprite angrySprite;
    public Sprite dissatisfactionSprite;
    public Sprite satisfactionSprite;
    private static int tutorialOrderIdx = 0;

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
        int idx;
        if (Managers.Game.isTutorial)
        {
            idx = tutorialOrderIdx;
            tutorialOrderIdx = (tutorialOrderIdx + 1) % foodList.Count; 
        }
        else
        {
            idx = Random.Range(0, foodList.Count);
        }
        selectedFood = foodList[idx];
        orderedImage.sprite = selectedFood.foodImage;
        Debug.Log($"º’¥‘¿Ã {selectedFood.foodName} ¡÷πÆ«‘!");
        if (currentTable != null)
        {
            currentTable.AssignPassenger(this);
        }
        yield break;
    }

    // ≈¿Â
    public void Exit(bool correctTable, int correctness,NodeRecipe recipe)
    {

        StartCoroutine(ExitRoutine(correctTable, correctness,recipe));
    }
    public void SetFoodList(List<FoodSO> foods)
    {
        foodList = foods;
    }

    IEnumerator ExitRoutine(bool correctTable, int correctness,NodeRecipe recipe)
    {
        if (correctTable && correctness == 1)
        {
            orderedImage.sprite = satisfactionSprite;
            Managers.Game.todaySelling += recipe.price;
            Managers.Game.playerTotalMoney += recipe.price;
            yield return new WaitForSeconds(2f);
        }else if(correctTable && correctness == 0)
        {
            orderedImage.sprite = dissatisfactionSprite;
            Managers.Game.todaySelling += recipe.price;
            Managers.Game.playerTotalMoney += recipe.price;
            yield return new WaitForSeconds(2f);
        }
        else
        {
            orderedImage.sprite = angrySprite;
            yield return new WaitForSeconds(2f);
        }

            selectedFood = null;
        Destroy(gameObject);
    }
}
