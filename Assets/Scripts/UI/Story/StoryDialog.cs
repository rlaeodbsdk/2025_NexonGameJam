using DG.Tweening;
using KoreanTyper;                                                  // Add KoreanTyper namespace | 네임 스페이스 추가
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
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
    private void Awake()
    {
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

                    if (scene.isFirstAppearance)
                    {
                        StandingImage[0].rectTransform.localScale = Vector3.one;
                        yield return new WaitForSecondsRealtime(2.2f);
                    }

                    else
                    {
                        StandingImage[0].rectTransform.localScale = new Vector3(0.6f, 0.6f, 0.6f);



                    }
                    if (scene.isAnger)
                    {
                        LeftCharacter.instance.frowningAnim.DORestartById("1");
                    }
                    if (scene.isSurprized)
                    {
                        LeftCharacter.instance.surprisedAnim.DORestartById("2");
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
                }
                else
                {
                    StandingImage[1].gameObject.SetActive(false);
                }

                // 패널 위치 (원래 위치 + 오프셋)
                if (panelRect != null)
                    panelRect.anchoredPosition = originalPanelPos + scene.panelPositionOffset;

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
                        RightCharacter.instance.frowningAnim.DORestartById("1");
                    }
                    if (scene.isSurprized)
                    {
                        RightCharacter.instance.surprisedAnim.DORestartById("2");
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

                // 입력받으면 TextPanel 끄기
                if (TextPanel != null)
                {
                    TextPanel.SetActive(false);
                    StandingImage[0].gameObject.SetActive(false);
                    StandingImage[1].gameObject.SetActive(false);
                }


                // 여기서 애니메이션(이동 등) 진행 시간 만큼 대기 (예: 2초)
                yield return new WaitForSecondsRealtime(2.0f); // 원하는 시간으로!

                bool panelTurnedOn = false;
                while (!panelTurnedOn)
                {
                    // 마우스 아무 버튼 클릭 시
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (TextPanel != null)
                            TextPanel.SetActive(true);

                        panelTurnedOn = true;
                    }
                    yield return null;
                }

                // 바로 다음 대사로!
                continue;
            }

            if (scene.isTimeGoing)
            {
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
                    Debug.Log($"[isTimeGoing] 경과시간: {elapsed:F2}초 / 목표: {targetDuration}초");
                    yield return null;
                }
                Time.timeScale = 0f;
                TextPanel.SetActive(true);
                continue;
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
        if (TutorialDialog != null)
        {
            TutorialDialog.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
        }

    }

}