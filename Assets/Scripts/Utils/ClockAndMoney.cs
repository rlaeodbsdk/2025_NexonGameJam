using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClockAndMoney : MonoBehaviour
{
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI timeText;

    public TextMeshProUGUI moneyText;

    public float currentTime = 30f;

    private void Start()
    {
        dayText.SetText("Day" + Managers.Game.roundNumber);
    }

    private void Update()
    {
        
        if (Time.timeScale != 0f && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0f)
            {
                currentTime = 0f;
                Managers.Game.roundNumber++;
                dayText.SetText("Day" + Managers.Game.roundNumber);
                Managers.Game.onRoundOver();
            }
                
        }

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timeText.SetText($"{minutes:00}:{seconds:00}");

        moneyText.SetText(Managers.Game.playerTotalMoney.ToString());
    }
}
