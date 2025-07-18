using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainGame : BaseScene   // MainGame 클래스는 BaseScene 클래스의 기능을 기반으로 게임 시작 시 필요한 초기화 작업을 수행하는 클래스
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
        Managers.Game.GameStart();
    }
}
