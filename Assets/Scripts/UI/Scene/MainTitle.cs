using UnityEngine;
using UnityEngine.EventSystems;
public class MainTitle : BaseScene
//MainTitle 클래스는 메인 메뉴 화면을 담당하는 클래스라고 할 수 있습니다. 마우스 클릭이나 터치를 감지하여 컷씬 UI를 보여주거나 주석 처리된 부분처럼 다른 씬으로 전환하는 역할을 수행할 수 있습니다. 

{
    public override void Clear()
    {
        throw new System.NotImplementedException();
    }

    protected override void Init()
    {
        base.Init();
    }

    private void Start()
    {
        Init();
        Managers.Sound.Play("BGM/titleBGM1",Define.Sound.BGM);
        Managers.UI.ShowPopUpUI<UI_MainScene>();
    }

    public void GameStart()
    {

    }
    public void GameQuit()
    {
     
    }

}
