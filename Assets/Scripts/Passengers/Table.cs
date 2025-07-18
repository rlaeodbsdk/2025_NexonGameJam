using UnityEngine;

public class Table : MonoBehaviour
{
    [Header("손님이 앉는 위치")]
    public Transform passengerPoint;

    [Header("이 테이블에 앉아있는 손님(없으면 null)")]
    public Passenger currentPassenger;

    [Header("현재 주문된 음식 (없으면 null)")]
    public FoodSO orderedFood;

    // 손님 배치
    public void AssignPassenger(Passenger passenger)
    {
        currentPassenger = passenger;
  
        passenger.transform.position = passengerPoint.position;
    }

    // 음식 주문 기록
    public void OrderFood(FoodSO food)
    {
        orderedFood = food;
       
    }

    // 테이블 리셋
    public void ResetTable()
    {
        currentPassenger = null;
        orderedFood = null;
    }
}