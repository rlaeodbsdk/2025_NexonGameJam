using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GoodEnding : UI_Popup
{
    // Start is called before the first frame update
    void Start()
    {
        Managers.Sound.Play("BGM/goodEnding", Define.Sound.BGM);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
