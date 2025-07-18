using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UI_Receipt : UI_Popup
{
    [Header("UI Elements")]
    public Image upDooroo;
    public Image middlePaper;
    public Image downDooroo;
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI InnerTexts;

    [Header("Animation Settings")]
    public float animationDuration = 1.0f;
    public float moveDistance = 300f;
    public float typeSpeed = 0.03f; // 글자 타이핑 속도

    [Header("Receipt Data")]
    public string title = "오늘의 영수증";
    public string totalSales = "120,000원";
    public string successCount = "23회";
    public string failCount = "2회";
    public string successRate = "92%";
    public string netProfit = "95,000원";
    public string rank = "S+";

    private Vector3 upStartPos;
    private Vector3 downStartPos;

    private bool skipTyping = false; // 클릭 시 전체 출력용

    void Start()
    {
        upStartPos = upDooroo.rectTransform.anchoredPosition;
        downStartPos = downDooroo.rectTransform.anchoredPosition;
        middlePaper.rectTransform.localScale = new Vector3(1f, 0f, 1f);

        PlayReceiptAnimation();
        StartCoroutine(ShowReceiptText());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            skipTyping = true;
        }
    }

    void PlayReceiptAnimation()
    {
        upDooroo.rectTransform.DOAnchorPosY(upStartPos.y + moveDistance, animationDuration)
    .SetEase(Ease.OutQuad).SetUpdate(true);
        downDooroo.rectTransform.DOAnchorPosY(downStartPos.y - moveDistance, animationDuration)
            .SetEase(Ease.OutQuad).SetUpdate(true);
        middlePaper.rectTransform.DOScaleY(10f, animationDuration)
            .SetEase(Ease.OutQuad).SetUpdate(true);
    }

    IEnumerator ShowReceiptText()
    {
        yield return new WaitForSecondsRealtime(animationDuration + 0.2f); // 애니 끝나고 시작

        // Title 타이핑
        yield return StartCoroutine(TypeText(TitleText, title));

        // InnerTexts 타이핑
        string fullText = $"오늘의 매출 금액 : {totalSales}\n" +
                          $"주문 성공한 횟수 : {successCount}\n" +
                          $"주문 실패한 횟수 : {failCount}\n" +
                          $"주문 성공률 : {successRate}\n" +
                          $"오늘의 순이익 : {netProfit}\n\n" +
                          $"등급 : {rank}";

        yield return StartCoroutine(TypeText(InnerTexts, fullText));
    }

    IEnumerator TypeText(TextMeshProUGUI textComponent, string fullText)
    {
        textComponent.text = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            if (skipTyping)
            {
                textComponent.text = fullText;
                yield break;
            }

            textComponent.text += fullText[i];
            yield return new WaitForSecondsRealtime(typeSpeed);
        }
    }
}
