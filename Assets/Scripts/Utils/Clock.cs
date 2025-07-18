using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI timeText;

    private float currentTime = 2f;

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
    }
}
