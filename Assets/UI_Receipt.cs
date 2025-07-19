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
    public string title = "일일정산";
    public string totalSales = "120,000원";
    public string successCount = "23회";
    public string failCount = "2회";
    public string successRate = "92%";
    public string netProfit = "95,000원";
    public string rank = "S+";

    private Vector3 upStartPos;
    private Vector3 downStartPos;
    private int count = 0;

    private bool skipTyping = false; // 클릭 시 전체 출력용

    void Start()
    {
        Time.timeScale = 0;
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

        Managers.Sound.Play("SFX/scrollExpand");
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
        yield return StartCoroutine(TypeText(TitleText, title,true));


        Managers.Game.playerTotalMoney -= Managers.Game.GetDiscountedIngredientPrice();
        // InnerTexts 타이핑
        string fullText = $"오늘의 매출 금액 : {Managers.Game.todaySelling}\n" +
                          $"주문 성공한 횟수 : {Managers.Game.completeOrderCount}\n" +
                          $"주문 실패한 횟수 : {Managers.Game.OrderCount- Managers.Game.completeOrderCount}\n" +
                          $"사용한 원재료 비용 : {-Managers.Game.totalIngredientMoney*Managers.Game.GetDiscountedIngredientPrice()}"+$"(할인률 {Managers.Game.ingredientDiscount*100}%)"+"\n" +
                          $"오늘의 순이익 : {Managers.Game.todaySelling-Managers.Game.totalIngredientMoney}\n\n" +
                          $"총 자본 : {Managers.Game.playerTotalMoney}";

        yield return StartCoroutine(TypeText(InnerTexts, fullText));
        
    }

    IEnumerator TypeText(TextMeshProUGUI textComponent, string fullText, bool isTitleText = false)
    {
      
        textComponent.text = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            if (skipTyping)
            {
                textComponent.text = fullText;
                StartCoroutine(End());
                yield break;
            }

            textComponent.text += fullText[i];

            if (isTitleText == true)
            {
                // "일일정산" 글자 하나마다 강렬한 사운드 재생
                Managers.Sound.Play("SFX/dailySettlement");
                yield return new WaitForSecondsRealtime(typeSpeed*24f); // 느리게 출력
            }
            else
            {
                Managers.Sound.Play("SFX/typing4");
                yield return new WaitForSecondsRealtime(typeSpeed);
            }
        }

        // 아래는 "count == 1"일 때 UI 애니메이션 처리
        if (count == 1)
        {
            yield return new WaitForSecondsRealtime(1.5f); // 대기
            TitleText.text = "";
            InnerTexts.text = "";

            // 두루마리와 종이를 원래 위치로 복구 (접히는 애니메이션)
            upDooroo.rectTransform.DOAnchorPosY(upStartPos.y, animationDuration * 0.5f)
                .SetEase(Ease.InQuad).SetUpdate(true);
            downDooroo.rectTransform.DOAnchorPosY(downStartPos.y, animationDuration * 0.5f)
                .SetEase(Ease.InQuad).SetUpdate(true);
            middlePaper.rectTransform.DOScaleY(0f, animationDuration * 0.5f)
                .SetEase(Ease.InQuad).SetUpdate(true);

            yield return new WaitForSecondsRealtime(animationDuration * 0.5f + 0.2f); // 접히는 애니 끝난 후

            // 전체 UI를 아래로 내려서 사라지게 하기
            RectTransform canvasRect = GetComponent<RectTransform>();
            if (canvasRect != null)
            {
                canvasRect.DOAnchorPosY(-Screen.height, 5f)
                    .SetEase(Ease.InCubic).SetUpdate(true);
            }

            // 애니메이션이 끝난 뒤 팝업 제거
            yield return new WaitForSecondsRealtime(1.0f);

            StartCoroutine(waitForNextStage());
         
        }

        count++;
    }


 

IEnumerator waitForNextStage()
{
    yield return new WaitForSecondsRealtime(2.1f);

    Managers.Game.GoNextStage();

    Destroy(this.gameObject);
}



IEnumerator End()
    {
        yield return new WaitForSecondsRealtime(1.5f); // 대기
        TitleText.text = "";
        InnerTexts.text = "";

        Managers.Sound.Play("SFX/scrollExpand");
        // 두루마리와 종이를 원래 위치로 복구 (접히는 애니메이션)
        upDooroo.rectTransform.DOAnchorPosY(upStartPos.y, animationDuration)
            .SetEase(Ease.InQuad).SetUpdate(true);
        downDooroo.rectTransform.DOAnchorPosY(downStartPos.y, animationDuration)
            .SetEase(Ease.InQuad).SetUpdate(true);
        middlePaper.rectTransform.DOScaleY(0f, animationDuration)
            .SetEase(Ease.InQuad).SetUpdate(true);
        yield return new WaitForSecondsRealtime(animationDuration + 0.2f); // 접히는 애니 끝난 후

        // 전체 UI를 아래로 내려서 사라지게 하기
        RectTransform canvasRect = GetComponent<RectTransform>();
        if (canvasRect != null)
        {
            canvasRect.DOAnchorPosY(-Screen.height, 5f)
                .SetEase(Ease.InCubic).SetUpdate(true);
        }

        // 애니메이션이 끝난 뒤 팝업 제거하거나 비활성화하려면 아래 코드 추가
        yield return new WaitForSecondsRealtime(1.0f);
        Managers.Game.openShop();
        Destroy(this.gameObject);
    }
}
