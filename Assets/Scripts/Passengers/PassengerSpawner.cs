using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PassengerSpawner : MonoBehaviour
{
    public float spawnTime = 4f;
    public Passenger passengerPrefab;
    public VillainPassenger villainPrefab;
    public TableManager tableManager;
    public Sprite[] passengerSprites;
    private int currentSpawnIndex = 0;
    private float timer = 0f;
    private bool isFirstSpawn = true;
    private float villainSpawnRate = 0f;

    private void Start()
    {
        switch(Managers.Game.roundNumber)
        {
            case 1: villainSpawnRate = 0f; break;
            case 2: villainSpawnRate = 0f; break;
            case 3: villainSpawnRate = Managers.Game.villainRate ; break;
        }
    }

    private void Update()
    {

        if (Time.timeScale != 0f && spawnTime > 0)
        {
            timer += Time.deltaTime;
            float targetTime = isFirstSpawn ? 2f : spawnTime;

            if (timer >= targetTime)
            {
                if(Managers.Game.isTutorial&&isFirstSpawn==false)
                {
                    return;
                }
                TrySpawnPassenger();
                timer = 0f;
                isFirstSpawn = false;
            }
        }

    }

    public void TrySpawnPassenger()
    {
        Debug.Log("Passenger Spawn");
        Table emptyTable = tableManager.GetRandomEmptyTable();
        if (emptyTable == null)
        {
            Debug.Log("�� ���̺� ����!");
            return;
        }
        if (Random.value < villainSpawnRate) // 10% Ȯ��
        {
            var villain = Instantiate(villainPrefab, emptyTable.villainPoint.position, Quaternion.identity, emptyTable.villainPoint);
            villain.Init(emptyTable);
            emptyTable.AssignVillain(villain);
            return;
        }
        // �մ� ����
        var passenger = Instantiate(
        passengerPrefab,
        emptyTable.passengerPoint.position, // ��ġ ����
        Quaternion.identity,
        emptyTable.passengerPoint
    );
        if (passengerSprites != null && passengerSprites.Length > 0)
        {
            int randIdx = Random.Range(0, passengerSprites.Length);
            var img = passenger.GetComponentInChildren<Image>();
            if (img != null)
                img.sprite = passengerSprites[randIdx];

        }
        passenger.SetFoodList(CustomShopManager.instance.foodList);

        passenger.Visit(emptyTable);  // currentTable ���� ��
    }
}
