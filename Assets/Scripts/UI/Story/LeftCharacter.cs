using DG.Tweening;
using UnityEngine;
using System;

public class LeftCharacter : MonoBehaviour
{
    public DOTweenAnimation frowningAnim;
    public DOTweenAnimation surprisedAnim;

    public static LeftCharacter instance;
    Vector3 prevScale;

    private void Awake()
    {
        instance = this;
    }
}
