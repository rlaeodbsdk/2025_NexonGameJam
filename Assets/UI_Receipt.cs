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
    public float typeSpeed = 0.03f; // ���� Ÿ���� �ӵ�

    [Header("Receipt Data")]
    public string title = "��������";
    public string totalSales = "120,000��";
    public string successCount = "23ȸ";
    public string failCount = "2ȸ";
    public string successRate = "92%";
    public string netProfit = "95,000��";
    public string rank = "S+";

    private Vector3 upStartPos;
    private Vector3 downStartPos;
    private int count = 0;

    private bool skipTyping = false; // Ŭ�� �� ��ü ��¿�

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
        yield return new WaitForSecondsRealtime(animationDuration + 0.2f); // �ִ� ������ ����

        // Title Ÿ����
        yield return StartCoroutine(TypeText(TitleText, title,true));


        Managers.Game.playerTotalMoney -= Managers.Game.GetDiscountedIngredientPrice();
        // InnerTexts Ÿ����
        string fullText = $"������ ���� �ݾ� : {Managers.Game.todaySelling}\n" +
                          $"�ֹ� ������ Ƚ�� : {Managers.Game.completeOrderCount}\n" +
                          $"�ֹ� ������ Ƚ�� : {Managers.Game.OrderCount- Managers.Game.completeOrderCount}\n" +
                          $"����� ����� ��� : {-Managers.Game.totalIngredientMoney*Managers.Game.GetDiscountedIngredientPrice()}"+$"(���η� {Managers.Game.ingredientDiscount*100}%)"+"\n" +
                          $"������ ������ : {Managers.Game.todaySelling-Managers.Game.totalIngredientMoney}\n\n" +
                          $"�� �ں� : {Managers.Game.playerTotalMoney}";

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
                // "��������" ���� �ϳ����� ������ ���� ���
                Managers.Sound.Play("SFX/dailySettlement");
                yield return new WaitForSecondsRealtime(typeSpeed*24f); // ������ ���
            }
            else
            {
                Managers.Sound.Play("SFX/typing4");
                yield return new WaitForSecondsRealtime(typeSpeed);
            }
        }

        // �Ʒ��� "count == 1"�� �� UI �ִϸ��̼� ó��
        if (count == 1)
        {
            yield return new WaitForSecondsRealtime(1.5f); // ���
            TitleText.text = "";
            InnerTexts.text = "";

            // �η縶���� ���̸� ���� ��ġ�� ���� (������ �ִϸ��̼�)
            upDooroo.rectTransform.DOAnchorPosY(upStartPos.y, animationDuration * 0.5f)
                .SetEase(Ease.InQuad).SetUpdate(true);
            downDooroo.rectTransform.DOAnchorPosY(downStartPos.y, animationDuration * 0.5f)
                .SetEase(Ease.InQuad).SetUpdate(true);
            middlePaper.rectTransform.DOScaleY(0f, animationDuration * 0.5f)
                .SetEase(Ease.InQuad).SetUpdate(true);

            yield return new WaitForSecondsRealtime(animationDuration * 0.5f + 0.2f); // ������ �ִ� ���� ��

            // ��ü UI�� �Ʒ��� ������ ������� �ϱ�
            RectTransform canvasRect = GetComponent<RectTransform>();
            if (canvasRect != null)
            {
                canvasRect.DOAnchorPosY(-Screen.height, 5f)
                    .SetEase(Ease.InCubic).SetUpdate(true);
            }

            // �ִϸ��̼��� ���� �� �˾� ����
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
        yield return new WaitForSecondsRealtime(1.5f); // ���
        TitleText.text = "";
        InnerTexts.text = "";

        Managers.Sound.Play("SFX/scrollExpand");
        // �η縶���� ���̸� ���� ��ġ�� ���� (������ �ִϸ��̼�)
        upDooroo.rectTransform.DOAnchorPosY(upStartPos.y, animationDuration)
            .SetEase(Ease.InQuad).SetUpdate(true);
        downDooroo.rectTransform.DOAnchorPosY(downStartPos.y, animationDuration)
            .SetEase(Ease.InQuad).SetUpdate(true);
        middlePaper.rectTransform.DOScaleY(0f, animationDuration)
            .SetEase(Ease.InQuad).SetUpdate(true);
        yield return new WaitForSecondsRealtime(animationDuration + 0.2f); // ������ �ִ� ���� ��

        // ��ü UI�� �Ʒ��� ������ ������� �ϱ�
        RectTransform canvasRect = GetComponent<RectTransform>();
        if (canvasRect != null)
        {
            canvasRect.DOAnchorPosY(-Screen.height, 5f)
                .SetEase(Ease.InCubic).SetUpdate(true);
        }

        // �ִϸ��̼��� ���� �� �˾� �����ϰų� ��Ȱ��ȭ�Ϸ��� �Ʒ� �ڵ� �߰�
        yield return new WaitForSecondsRealtime(1.0f);
        Managers.Game.openShop();
        Destroy(this.gameObject);
    }
}
