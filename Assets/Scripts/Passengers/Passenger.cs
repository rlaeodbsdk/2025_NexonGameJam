using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    private List<FoodSO> foodList;
    public FoodSO selectedFood;
    private CustomTableManager customTableManager;

    private void OnEnable()
    {
        customTableManager = FindFirstObjectByType<CustomTableManager>();
        Visit(1);
    }
    public void Visit(int tableNumber) { }
    public void Order(FoodSO food, int tableNumber)
    {
        //음식선택, 가능음식 중 랜덤
        int randomValidFood = Random.Range(0, foodList.Count);
        selectedFood = foodList[randomValidFood];
        //음식주문
        //OrderFood(selectedFood);
    }
    public void Exit() { selectedFood = null; }

    
}
