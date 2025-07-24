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
        Managers.Sound.Play("SFX/guestAppearance");
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
            idx = 0;
        }
        else
        {
            idx = Random.Range(0, foodList.Count);
        }
        selectedFood = foodList[idx];
        foodList.RemoveAt(idx); // SelectedFood를 방문객이 온 동안 리스트에서 제외함, 다른 방문객은 선택불가
        orderedImage.sprite = selectedFood.foodImage;
        Debug.Log($"손님이 {selectedFood.foodName} 주문함!");
        if (currentTable != null)
        {
            currentTable.AssignPassenger(this);
        }
        yield break;
    }

    // 퇴장
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
            Managers.Sound.Play("SFX/guestSatisfied");
            orderedImage.sprite = satisfactionSprite;
            orderedImage.gameObject.transform.localScale = new Vector3 (3.3f, 2.5f, 1f);
            Managers.Game.todaySelling += recipe.price;
            Managers.Game.playerTotalMoney += recipe.price;
            Managers.Game.completeOrderCount++;

            yield return new WaitForSeconds(2f);
        }else if(correctTable && correctness == 0)
        {
            Managers.Sound.Play("SFX/guestDissatisfied");
            orderedImage.sprite = dissatisfactionSprite;
            orderedImage.gameObject.transform.localScale = new Vector3(3.3f, 2.5f, 1f);
            Managers.Game.todaySelling += recipe.price;
            Managers.Game.playerTotalMoney += recipe.price;
            yield return new WaitForSeconds(2f);
        }
        else
        {
            Managers.Sound.Play("SFX/guestAngry");
            orderedImage.sprite = angrySprite;
            orderedImage.gameObject.transform.localScale = new Vector3(3.3f, 2.5f, 1f);
            yield return new WaitForSeconds(2f);
        }
        orderedImage.gameObject.transform.localScale = new Vector3(2.5f, 2.5f, 1f);
        foodList.Add(selectedFood); //selectedFood를 다시 리스트에 넣어서 다른 객이 선택가능하게 함
        selectedFood = null;
        Destroy(gameObject);
    }
}
