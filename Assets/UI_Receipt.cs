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
    public float moveDistance = 300f; // �η縶���� �̵��� �Ÿ�
    private Vector3 upStartPos;
    private Vector3 downStartPos;

    void Start()
    {
        // ���� ��ġ ���
        upStartPos = upDooroo.rectTransform.anchoredPosition;
        downStartPos = downDooroo.rectTransform.anchoredPosition;

        // middlePaper �ʱ� ������ ����
        middlePaper.rectTransform.localScale = new Vector3(1f, 0f, 1f);

        PlayReceiptAnimation();
    }

    void PlayReceiptAnimation()
    {
        // �� �η縶�� ���� �̵�
        upDooroo.rectTransform.DOAnchorPosY(upStartPos.y + moveDistance, animationDuration).SetEase(Ease.OutQuad);

        // �Ʒ� �η縶�� �Ʒ��� �̵�
        downDooroo.rectTransform.DOAnchorPosY(downStartPos.y - moveDistance, animationDuration).SetEase(Ease.OutQuad);

        // ��� ���� y������ Ȯ��
        middlePaper.rectTransform.DOScaleY(10f, animationDuration).SetEase(Ease.OutQuad);
    }
}
