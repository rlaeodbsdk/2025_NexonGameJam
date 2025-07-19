using System.Collections;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    public float[] spawnTimes; 
    public Passenger passengerPrefab;
    public TableManager tableManager; 

    private int currentSpawnIndex = 0;
    private float timer = 0f;

    private void Update()
    {
        if (currentSpawnIndex >= spawnTimes.Length)
            return;

        timer += Time.deltaTime;
        if (timer >= spawnTimes[currentSpawnIndex])
        {
            TrySpawnPassenger();
            currentSpawnIndex++;
        }
    }

    public void TrySpawnPassenger()
    {
        Debug.Log("Passenger Spawn");
        Table emptyTable = tableManager.GetRandomEmptyTable();
        if (emptyTable == null)
        {
            Debug.Log("빈 테이블 없음!");
            return;
        }

        // 손님 생성
        var passenger = Instantiate(
        passengerPrefab,
        emptyTable.passengerPoint.position, // 위치 지정
        Quaternion.identity,
        emptyTable.passengerPoint
    );

        passenger.SetFoodList(CustomShopManager.instance.foodList);

        passenger.Visit(emptyTable);  // currentTable 세팅 등
    }
}
