using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Table : MonoBehaviour
{

    public int tableNumber; // �����̺��� ������ ������ ID
    [Header("�մ��� �ɴ� ��ġ")]
    public Transform passengerPoint;

    [Header("�� ���̺� �ɾ��ִ� �մ�(������ null)")]
    public Passenger currentPassenger;

    [Header("���� �ֹ��� ���� (������ null)")]
    public FoodSO orderedFood;

    public Image dishImage;
    
    // �մ� ��ġ
    public void AssignPassenger(Passenger passenger)
    {
        currentPassenger = passenger; // Ư���մ��� �ִ� Table
       

        var nodeManager = FindFirstObjectByType<NodeManager>();
        nodeManager.NodeGo(tableNumber, passenger.selectedFood.foodNodeName, this);


        passenger.transform.position = passengerPoint.position;
    }

    // ���� �ֹ� ���
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