using UnityEngine;

public class Table : MonoBehaviour
{

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