using DG.Tweening;
using KoreanTyper;                                                  // Add KoreanTyper namespace | 네임 스페이스 추가
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDialog : UI_Popup
{
    public Text[] TestTexts;
    public Image[] StandingImage;
    public CharacterData[] cd;
    public GameObject TextPanel;
    public DOTweenAnimation[] StandingAnimations;

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
        StartCoroutine(TypingCoroutine());
    }

    public IEnumerator TypingCoroutine()
    {
        panelRect.anchoredPosition = originalPanelPos;

        for (int idx = 0; idx < scenes.Count; idx++)
        {

            DialogueScene scene = scenes[idx];

            yield return new WaitForSeconds(scene.preDelay);

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

                    if (scene.isFirstAppearance)
                    {
                        StandingImage[0].rectTransform.localScale = Vector3.one;
                        yield return new WaitForSeconds(2.2f);
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

            
            TestTexts[idx].gameObject.SetActive(true);
            string full = scene.text;
            int len = full.GetTypingLength();

            for (int i = 0; i <= len; i++)
            {
                TestTexts[idx].text = full.Typing(i);
                yield return new WaitForSeconds(0.025f);
            }

            if (scene is TutorialDialogScene tutorialScene && tutorialScene.requiredKey != KeyCode.None)
            {
                while (!Input.GetKeyDown(tutorialScene.requiredKey))
                {
                    yield return null;
                }
            }
            else
            {
                // 일반 대사 : Space/Enter로 넘어감
                while (!Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Return))
                {
                    yield return null;
                }
            }

            yield return new WaitForSeconds(scene.postDelay);
        }
        contents.SetActive(false);
    }
            
}