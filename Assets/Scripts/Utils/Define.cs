using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum IllustType
    { 
        Start,
        Phase1End,
        Phase2End,
        Fail,
        Success,
        Num
    }
  
    public enum ItemType
    {
        Riffle,
        FireFlame,
        Launcher,
        CartridgeBelt
    }
    public enum Rarity
    {
        Normal,
        Rare,
        Epic,
        Legend
    }

    public enum UseType
    {
        Active,
        Passive
    }
    public enum WorldObject
    {
        Unknown,
        Player,
        Enemy,
    }
    public enum State
    {
        Idle,
        Walk,
        Crouch
    }
    public enum UIEvent
    {
        Click,
        BeginDrag,
        Drag,
        DragEnd,
        PointerDown,
        PointerUP
    }
    public enum MouseEvent
    {
        Press,
        Click,
        End
    }
    public enum Scene
    {
        Unknown,
        TitleScene,
        MainGame,
        MainTitle,
        GameScene
    }
    public enum Sound
    {
        Master,
        BGM,
        SFX,
        MaxCount
    }
}