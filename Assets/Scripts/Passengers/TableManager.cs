using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableManager : MonoBehaviour
{
    public GameObject tableParent;  
    public GameObject tablePrefab;

    private DOTweenAnimation tableAnim;
    public List<Table> tables = new List<Table>();
    private int tableCount = 0;
    private int Count = 0;

    private HorizontalLayoutGroup layoutGroup;

    private void Awake()
    {
        layoutGroup = tableParent.GetComponent<HorizontalLayoutGroup>();
        tables.Clear();
        foreach (var table in FindObjectsByType<Table>(FindObjectsSortMode.None))
        {
            tables.Add(table);
            table.SetTable(Count++);
        }
        if (tables.Count == 0)
        {
            AddTable();
        }
    }

    // 호출: 테이블을 구매할 때
    public void AddTable()
    {
        
        var newTable = Instantiate(tablePrefab, tableParent.transform);
        var tableComp = newTable.GetComponent<Table>();


        tables.Add(tableComp);
        tableComp.SetTable(Count++);
        var tableAnim = newTable.GetComponent<DOTweenAnimation>();
        if (tableAnim != null)
        {
            tableAnim.DORestart();
        }

        tableCount++;

    }
    public Table GetRandomEmptyTable()
    {
        List<Table> emptyTables = tables.FindAll(
        t => t.currentPassenger == null && t.currentVillain == null
    );
        if (emptyTables.Count == 0) return null;
        return emptyTables[Random.Range(0, emptyTables.Count)];
    }

 
}
