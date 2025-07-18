using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UI_MainScene : UI_Popup
{
    enum Buttons
    {
        StartButton,
        EndButton
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.StartButton).gameObject.AddUIEvent(StartClicked);
        GetButton((int)Buttons.EndButton).gameObject.AddUIEvent(EndClicked);

    }

    void StartClicked(PointerEventData eventData)
    {
        Debug.Log("게임시작");
    }

    void EndClicked(PointerEventData eventData)
    {
        Debug.Log("게임종료");
        
    }
}
