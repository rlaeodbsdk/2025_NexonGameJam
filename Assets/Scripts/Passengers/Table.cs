using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Table : MonoBehaviour
{

    public int tableNumber; // 각테이블이 가지는 고유한 ID
    [Header("손님이 앉는 위치")]
    public Transform passengerPoint;

    [Header("이 테이블에 앉아있는 손님(없으면 null)")]
    public Passenger currentPassenger;

    [Header("현재 주문된 음식 (없으면 null)")]
    public FoodSO orderedFood;

    public Image dishImage;
    
    // 손님 배치
    public void AssignPassenger(Passenger passenger)
    {
        currentPassenger = passenger; // 특정손님이 있는 Table
       

        var nodeManager = FindFirstObjectByType<NodeManager>();
        nodeManager.NodeGo(tableNumber, passenger.selectedFood.foodNodeName, this);


        passenger.transform.position = passengerPoint.position;
    }

    // 음식 주문 기록
    public void OrderFood(FoodSO food)
    {
        orderedFood = food;
       
    }

    public void ReceivedFood(Sprite foodImage, bool correctTable)
    {
        if (correctTable)
        {
            StartCoroutine(ReceivedFoodRoutine(foodImage));
        }
    }
    
    IEnumerator ReceivedFoodRoutine(Sprite foodImage)
    {
        dishImage.sprite = foodImage;
        dishImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        dishImage.gameObject.SetActive(false);
        ResetTable();
        yield break;
    }

    // 테이블 리셋
    public void ResetTable()
    {
        currentPassenger = null;
        orderedFood = null;
    }
    
    // 테이블 id 등록
    public void SetTable(int id)
    {
        tableNumber = id;
    }
}