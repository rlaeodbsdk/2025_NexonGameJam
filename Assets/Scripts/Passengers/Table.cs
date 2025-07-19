using UnityEngine;

public class Table : MonoBehaviour
{
    private NodeManager nodeManager;
    private int tableNumber; // �����̺��� ������ ������ ID
    [Header("�մ��� �ɴ� ��ġ")]
    public Transform passengerPoint;

    [Header("�� ���̺� �ɾ��ִ� �մ�(������ null)")]
    public Passenger currentPassenger;

    [Header("���� �ֹ��� ���� (������ null)")]
    public FoodSO orderedFood;
    
    // �մ� ��ġ
    public void AssignPassenger(Passenger passenger)
    {
        currentPassenger = passenger;
        if (nodeManager == null)
            Debug.LogError("nodeManager is NULL!!!");

        if (passenger == null)
            Debug.LogError("passenger is NULL!!!");

        if (passenger.selectedFood == null)
            Debug.LogError("passenger.selectedFood is NULL!!!");

        nodeManager = FindFirstObjectByType<NodeManager>();
        nodeManager.NodeGo(passenger.selectedFood.foodName);


        passenger.transform.position = passengerPoint.position;
    }

    // ���� �ֹ� ���
    public void OrderFood(FoodSO food)
    {
        orderedFood = food;
       
    }

    // ���̺� ����
    public void ResetTable()
    {
        currentPassenger = null;
        orderedFood = null;
    }
    
    // ���̺� id ���
    public void SetTable(int id)
    {
        tableNumber = id;
    }
}