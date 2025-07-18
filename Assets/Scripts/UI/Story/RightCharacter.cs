using DG.Tweening;
using UnityEngine;

public class RightCharacter : MonoBehaviour
{
    public DOTweenAnimation frowningAnim;
    public DOTweenAnimation surprisedAnim;

    public static RightCharacter instance;

    private void Awake()
    {
        instance = this;
    }
}
