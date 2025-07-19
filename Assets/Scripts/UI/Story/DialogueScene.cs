using DG.Tweening;
using UnityEngine;

public enum DialogueCharacter
{
    HideDialog,
    None,      
    헨젤,
    그레텔,
    콩쥐,
    팥쥐맘,
    계모,
    할머니
}

[System.Serializable]
public class DialogueScene
{
    [Header("대사 주인공 선택 (None은 배경대사)")]
    public DialogueCharacter speakingCharacter;
    public bool isFirstAppearance;
    [TextArea]
    public string text;
    public Sprite overrideSprite;    
    public Vector2 panelPositionOffset;
    public float preDelay;            
    public float postDelay;        
    public bool showLeftCharacter;
    public bool showRightCharacter;
    public bool isAnger;
    public bool isSurprized;
    public bool leftSDAnim;
    public bool rightSDAnim;
    public KeyCode requiredKey;
}