using DG.Tweening;
using KoreanTyper;                                                  // Add KoreanTyper namespace | 네임 스페이스 추가
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryDialog : UI_Popup
{
    public Text[] TestTexts;
    public Image[] StandingImage;
    public CharacterData[] cd;
    public GameObject TextPanel;
    public DOTweenAnimation[] StandingAnimations;
    public GameObject dimmedPanel;
    public GameObject TutorialDialog;

    public Vector2 panelOffset;
    public GameObject contents;

    private RectTransform panelRect;
    private Vector2 originalPanelPos;
    public List<DialogueScene> scenes;

    public bool canGoNextStep = true;
    public CustomShopManager shopManager;
    public GameObject shop;

    public GameObject[] leftSDAnimSet;        
    public GameObject leftSDCharacter;
    public GameObject KongCanvas;

    public GameObject[] rightSDAnimSet;
    public GameObject rightSDCharacter;
    public GameObject GretCanvas;
    private void Awake()
    {
        shopManager = FindObjectOfType<CustomShopManager>();
        panelRect = TextPanel.GetComponent<RectTransform>();
        originalPanelPos = panelRect.anchoredPosition;
        contents.SetActive(true);
    }
        
    private void OnEnable()
    {
        Time.timeScale = 0f;
        Managers.Game.isTutorial = true; 
        StartCoroutine(TypingCoroutine());
    }

    public IEnumerator TypingCoroutine()
    {
        panelRect.anchoredPosition = originalPanelPos;

        for (int idx = 0; idx < scenes.Count; idx++)
        {

            DialogueScene scene = scenes[idx];

            yield return new WaitForSecondsRealtime(scene.preDelay);

            // ======================
            // 0. Hide Dialog 처리
            // ======================
            if (scene.speakingCharacter == DialogueCharacter.HideDialog)
            {
                TextPanel.SetActive(false);
                if (StandingImage != null)
                {
                    foreach (var standing in StandingImage)
                        standing.gameObject.SetActive(false);
                }
            }

            // ======================
            // 1. 캐릭터 None 처리
            // ======================
            if (scene.speakingCharacter == DialogueCharacter.None)
            {
                // 캐릭터 이미지 모두 OFF
                StandingImage[0].gameObject.SetActive(false);
                StandingImage[1].gameObject.SetActive(false);
                // 패널 원래 위치 (panelPositionOffset 무시)
                if (panelRect != null)
                    panelRect.anchoredPosition = originalPanelPos;
            }
            else
            {
                // ======================
                // 2. 일반 캐릭터 처리
                // ======================
                // 캐릭터 인덱스 None 제외
                int charIdx = (int)scene.speakingCharacter - 2;

                // 왼쪽 캐릭터 처리
                if (scene.showLeftCharacter)
                {

                    StandingImage[0].sprite = scene.overrideSprite != null ? scene.overrideSprite : cd[charIdx].CharacterImage;
                    StandingImage[0].gameObject.SetActive(true);
                    StandingImage[0].SetNativeSize();

                    if (scene.XFlip)
                    {
                        LeftCharacter.instance.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else
                    {
                        LeftCharacter.instance.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
                    }

                    if (scene.isFirstAppearance)
                    {
                        StandingImage[0].rectTransform.localScale = Vector3.one;

                        yield return new WaitForSecondsRealtime(2.2f);
                        TextPanel.SetActive(true);
                    }
                    else
                    {

                    }
                    if (scene.isAnger)
                    {
                        Debug.Log("왼쪽사람화내기!");
                        LeftCharacter.instance.frowningAnim.DORestartById("1");
                    }
                    if (scene.isSurprized)
                    {
                        Debug.Log("왼쪽사람놀라기!");
                        LeftCharacter.instance.frowningAnim.DORestartById("2");
                    }
                }

                else
                {
                    StandingImage[0].gameObject.SetActive(false);
                }

                // 오른쪽 캐릭터 처리
                if (scene.showRightCharacter)
                {
                   
                    StandingImage[1].sprite = scene.overrideSprite != null ? scene.overrideSprite : cd[charIdx].CharacterImage;
                    StandingImage[1].gameObject.SetActive(true);
                    StandingImage[1].SetNativeSize();
                    if (scene.XFlip)
                    {
                        RightCharacter.instance.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else
                    {
                        RightCharacter.instance.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
                    }

                }
                else
                {
                    StandingImage[1].gameObject.SetActive(false);
                }

                // 패널 위치 (원래 위치 + 오프셋)
                if (panelRect != null)
                    panelRect.anchoredPosition = originalPanelPos + scene.panelPositionOffset;

                if (scene.showRightCharacter)
                {
                    if (scene.isFirstAppearance)
                    {

                        if (TextPanel != null)
                            TextPanel.SetActive(false);


                        if (TextPanel != null)
                            TextPanel.SetActive(true);

                    }
                    else
                    {
                        if (scene.isAnger)
                        {
                            Debug.Log("오른쪽사람화내기!");
                            RightCharacter.instance.frowningAnim.DORestartById("1");
                        }
                        if (scene.isSurprized)
                        {
                            Debug.Log("오른쪽사람놀라기!");
                            RightCharacter.instance.surprisedAnim.DORestartById("2");
                        }
                    }
                }
            }

            // 텍스트 표시 (타이핑 효과)
            TestTexts[idx].gameObject.SetActive(true);
            string full = scene.text;
            int len = full.GetTypingLength();

            for (int i = 0; i <= len; i++)
            {
                TestTexts[idx].text = full.Typing(i);
                yield return new WaitForSecondsRealtime(0.025f);
            }

            if (scene.leftSDAnim || scene.rightSDAnim)
            {
                if (scene.requiredKey == KeyCode.None)
                {
                    while ((!Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Return)))
                    {
                        TestTexts[idx].text = full;
                        yield return null;
                    }

                }
                else
                {
                    while (!Input.GetKeyDown(scene.requiredKey))
                    {
                        TestTexts[idx].text = full;
                        yield return null;
                    }
                }

                // 입력받으면 TextPanel 끄기
                if (TextPanel != null)
                {
                    TextPanel.SetActive(false);
                    StandingImage[0].gameObject.SetActive(false);
                    StandingImage[1].gameObject.SetActive(false);
                }

                if (scene.leftSDAnim)
                {
                    var leftAnim = leftSDCharacter.GetComponent<DOTweenAnimation>();
                    var leftSR = leftSDCharacter.GetComponent<SpriteRenderer>();
                    leftSR.sortingOrder = 3;
                    if (leftAnim != null)
                        leftAnim.DORestart();


                    foreach (var obj in leftSDAnimSet)
                        if (obj != null) obj.SetActive(true);

                   
                }
                if (scene.rightSDAnim)
                {
                    var rightAnim = rightSDCharacter.GetComponent<DOTweenAnimation>();
                    var rightSR = rightSDCharacter.GetComponent<SpriteRenderer>();
                    rightSR.sortingOrder = 3;
                    if (rightAnim != null)
                        rightAnim.DORestart();


                    foreach (var obj in rightSDAnimSet)
                        if (obj != null) obj.SetActive(true);


                }

                yield return new WaitForSecondsRealtime(1.5f);
                if (scene.leftSDAnim) KongCanvas.SetActive(true);
                if (scene.rightSDAnim) GretCanvas.SetActive(true);
                bool panelTurnedOn = false;
                while (!panelTurnedOn)
                {
                    // 마우스 아무 버튼 클릭 시
                    if (Input.GetMouseButtonDown(0))
                    {
                        

                        if (KongCanvas != null) KongCanvas.SetActive(false);
                        if (GretCanvas != null) GretCanvas.SetActive(false);
                        if (scene.leftSDAnim)
                        {
                            foreach (var obj in leftSDAnimSet)
                                if (obj != null) obj.SetActive(false);
                            var leftAnim = leftSDCharacter.GetComponent<DOTweenAnimation>();
                            var leftSR = leftSDCharacter.GetComponent<SpriteRenderer>();
                            leftAnim.DOPlayBackwards();
                            leftSR.sortingOrder = -3;
                        }
                        if(scene.rightSDAnim)
                        {
                            foreach (var obj in rightSDAnimSet)
                                if (obj != null) obj.SetActive(false);
                            var rightAnim = rightSDCharacter.GetComponent<DOTweenAnimation>();
                            var rightSR = rightSDCharacter.GetComponent<SpriteRenderer>();
                            rightAnim.DOPlayBackwards();
                            rightSR.sortingOrder = -3;
                        }
                            
                        
                        if (TextPanel != null)
                            TextPanel.SetActive(true);

                        panelTurnedOn = true;
                    }
                    yield return null;
                }

                // 바로 다음 대사로!
                continue;
            }
            if (scene.isTomatoSoupGo)
            {
                FindFirstObjectByType<PassengerSpawner>().TrySpawnPassenger();
            }

            if(scene.isShopingGo)
            {
                StartCoroutine(goShopingOn());
            }

            if(scene.isGivingMoney)
            {
                Managers.Game.playerTotalMoney -= scene.givingMoneyAmount;
            }

            if (scene.isTimeGoing)
            {
                Managers.Game.canControl = false;
                while (TestTexts[idx].text != full)
                    yield return null;

                Managers.Game.canControl = true;
                if (scene.requiredKey == KeyCode.None)
                {
                    while ((!Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Return)))
                    {
                        TestTexts[idx].text = full;
                        yield return null;
                    }

                }
                else
                {
                    while (!Input.GetKeyDown(scene.requiredKey))
                    {
                        TestTexts[idx].text = full;
                        yield return null;
                    }
                }
                if (TextPanel != null)
                {
                    TextPanel.SetActive(false);
                    StandingImage[0].gameObject.SetActive(false);
                    StandingImage[1].gameObject.SetActive(false);
                }

                Time.timeScale = 1f;
                float startTime = Time.unscaledTime;
                float targetDuration = scene.goingTimeAmount;
                float elapsed = 0f;
                while (elapsed < targetDuration)
                {
                    elapsed = Time.unscaledTime - startTime;
                    yield return null;
                }
                Time.timeScale = 0f;
                TextPanel.SetActive(true);
                continue;
            }

        
            if (scene.isEnding==true)
            {
                float money = Managers.Game.playerTotalMoney;
                if(money>=0)
                {
                    Managers.UI.ShowPopUpUI<UI_GoodEnding>();
                    Debug.Log("굿엔딩");
                }
                else if((money<0)||(money>=-1000))
                {
                    Managers.UI.ShowPopUpUI<UI_GoodEnding>();
                    Debug.Log("노말엔딩");
                }
                else if(money<-1000)
                 {
                    Managers.UI.ShowPopUpUI<UI_BadEnding>();
                 Debug.Log("배드엔딩");

                }
            }
            if(scene.isEndDialogue==true)
            {
                StartCoroutine(IsEndGo());
            }
            if (scene.requiredKey == KeyCode.None)
            {
                while (!Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Return))
                {
                    TestTexts[idx].text = full;
                    yield return null;
                }
            }
            else
            {
                while (!Input.GetKeyDown(scene.requiredKey))
                {
                    TestTexts[idx].text = full;
                    yield return null;
                }
            }


            yield return new WaitForSecondsRealtime(scene.postDelay);
        }
        if (dimmedPanel != null)
        {
            StandingImage[0].gameObject.SetActive(false);
            StandingImage[1].gameObject.SetActive(false);
            TextPanel.SetActive(false);
            dimmedPanel.GetComponent<DOTweenAnimation>().DORestart();
            yield return new WaitForSecondsRealtime(1f);
        }

        contents.SetActive(false);
        Managers.Game.isTutorial = false;
        Time.timeScale = 1f;

        if (TutorialDialog != null)
        {
            TutorialDialog.SetActive(true);
        }
        else
        {

        }

    }
    IEnumerator goShopingOn()
    {
        canGoNextStep = false;
        shop.SetActive(true);
        Managers.Sound.Play("BGM/storeBGM1", Define.Sound.BGM);
        shopManager.OnShopClosed += OnShopClosedHandler; // 구독
        yield return new WaitForSecondsRealtime(2f);
        TextPanel.SetActive(false);

    }

    void OnShopClosedHandler()
    {
        // 다시 필요한 동작 수행
        Debug.Log("Shop closed! Resume logic.");

        shopManager.OnShopClosed -= OnShopClosedHandler;  // 구독 해제 (중요)

        StartCoroutine(ResumeAfterShop()); // 원하는 로직 이어가기
    }

    IEnumerator ResumeAfterShop()
    {
        yield return new WaitForSeconds(1f);
        canGoNextStep = true;
        TextPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    IEnumerator IsEndGo()
    {
        yield return new WaitForSecondsRealtime(1f);
        Managers.UI.ShowPopUpUI<UI_Receipt>();
    }


}