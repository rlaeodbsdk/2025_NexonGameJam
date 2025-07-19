using System.Collections;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    public float spawnTime = 4f;
    public Passenger passengerPrefab;
    public TableManager tableManager; 

    private int currentSpawnIndex = 0;
    private float timer = 0f;
    private bool isFirstSpawn = true;


    private void Update()
    {

        if (Time.timeScale != 0f && spawnTime > 0)
        {
            timer += Time.deltaTime;
            float targetTime = isFirstSpawn ? 2f : spawnTime;

            if (timer >= targetTime)
            {
                TrySpawnPassenger();
                timer = 0f;
                isFirstSpawn = false;
            }
        }

    }

    void TrySpawnPassenger()
    {
        Table emptyTable = tableManager.GetRandomEmptyTable();
        if (emptyTable == null)
        {
            Debug.Log("�� ���̺� ����!");
            return;
        }

        // �մ� ����
        var passenger = Instantiate(
        passengerPrefab,
        emptyTable.passengerPoint.position, // ��ġ ����
        Quaternion.identity,
        emptyTable.passengerPoint
    );

        passenger.SetFoodList(CustomShopManager.instance.foodList);

        passenger.Visit(emptyTable);  // currentTable ���� ��
    }
}
