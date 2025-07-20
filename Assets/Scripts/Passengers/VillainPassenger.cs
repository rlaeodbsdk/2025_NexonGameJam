using UnityEngine;
using UnityEngine.UI;
public class VillainPassenger : MonoBehaviour
{
    private Table currentTable;
    public GameObject sleepingPanel;
    public Image sleepingImage;
    public Sprite sleepingSprite;

    public void Init(Table table)
    {
        currentTable = table;
        sleepingImage.sprite = sleepingSprite;
        sleepingPanel.SetActive(true);
    }

    // UI Button Ŭ���� ȣ��
    public void OnClickExpel()
    {
        currentTable.ExpelVillain();
    }
}