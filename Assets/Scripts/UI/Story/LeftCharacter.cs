using DG.Tweening;
using UnityEngine;

public class LeftCharacter : MonoBehaviour
{
    public DOTweenAnimation frowningAnim;
    public DOTweenAnimation surprisedAnim;

    public static LeftCharacter instance;

    private void Awake()
    {
        instance = this;
    }
}
