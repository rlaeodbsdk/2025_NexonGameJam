using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        // TitleScene 초기화 코드
        Debug.Log("TitleScene Initialized");
    }

    public override void Clear()
    {
        // TitleScene 종료/정리 작업
        Debug.Log("TitleScene Cleared");
    }
}
