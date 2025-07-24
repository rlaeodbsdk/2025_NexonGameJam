using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UI_MainScene : UI_Popup
{
    enum Buttons
    {
        StartButton
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        //Managers.Sound.Play("BGM/titleBGM1");
        base.Init();
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.StartButton).gameObject.AddUIEvent(StartClicked);

    }

    void StartClicked(PointerEventData eventData)
    {
        Managers.Sound.Play("SFX/buttonClick1");
        Managers.Sound.StopBGM();
        Managers.Scene.LoadScene(Define.Scene.GameScene);
        
        Managers.Sound.Play("BGM/stageBGM1", Define.Sound.BGM);
    }

}
