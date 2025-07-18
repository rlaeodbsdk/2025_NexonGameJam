using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Receipt : UI_Popup
{
    public Image upDooroo;
    public Image middlePaper;
    public Image downDooroo;
    public float animationDuration = 1.0f;
    public float moveDistance = 300f; // 두루마리가 이동할 거리
    private Vector3 upStartPos;
    private Vector3 downStartPos;

    void Start()
    {
        // 시작 위치 기억
        upStartPos = upDooroo.rectTransform.anchoredPosition;
        downStartPos = downDooroo.rectTransform.anchoredPosition;

        // middlePaper 초기 스케일 설정
        middlePaper.rectTransform.localScale = new Vector3(1f, 0f, 1f);

        PlayReceiptAnimation();
    }

    void PlayReceiptAnimation()
    {
        // 위 두루마리 위로 이동
        upDooroo.rectTransform.DOAnchorPosY(upStartPos.y + moveDistance, animationDuration).SetEase(Ease.OutQuad);

        // 아래 두루마리 아래로 이동
        downDooroo.rectTransform.DOAnchorPosY(downStartPos.y - moveDistance, animationDuration).SetEase(Ease.OutQuad);

        // 가운데 종이 y스케일 확장
        middlePaper.rectTransform.DOScaleY(10f, animationDuration).SetEase(Ease.OutQuad);
    }
}
