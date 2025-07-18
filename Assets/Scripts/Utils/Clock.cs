using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI timeText;

    private Time time;

    private void Start()
    {
        
    }

    private void Update()
    {
        dayText.SetText("Day" );
    }
}
