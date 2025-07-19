using DG.Tweening;
using KoreanTyper;                                                  // Add KoreanTyper namespace | ���� �����̽� �߰�
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
            // 0. Hide Dialog ó��
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
            // 1. ĳ���� None ó��
            // ======================
            if (scene.speakingCharacter == DialogueCharacter.None)
            {
                // ĳ���� �̹��� ��� OFF
                StandingImage[0].gameObject.SetActive(false);
                StandingImage[1].gameObject.SetActive(false);
                // �г� ���� ��ġ (panelPositionOffset ����)
                if (panelRect != null)
                    panelRect.anchoredPosition = originalPanelPos;
            }
            else
            {
                // ======================
                // 2. �Ϲ� ĳ���� ó��
                // ======================
                // ĳ���� �ε��� None ����
                int charIdx = (int)scene.speakingCharacter - 2;

                // ���� ĳ���� ó��
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

                // ������ ĳ���� ó��
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

                // �г� ��ġ (���� ��ġ + ������)
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

            // �ؽ�Ʈ ǥ�� (Ÿ���� ȿ��)
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

                // �Է¹����� TextPanel ����
                if (TextPanel != null)
                {
                    TextPanel.SetActive(false);
                    StandingImage[0].gameObject.SetActive(false);
                    StandingImage[1].gameObject.SetActive(false);
                }


                // ���⼭ �ִϸ��̼�(�̵� ��) ���� �ð� ��ŭ ��� (��: 2��)
                yield return new WaitForSecondsRealtime(2.0f); // ���ϴ� �ð�����!

                bool panelTurnedOn = false;
                while (!panelTurnedOn)
                {
                    // ���콺 �ƹ� ��ư Ŭ�� ��
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (TextPanel != null)
                            TextPanel.SetActive(true);

                        panelTurnedOn = true;
                    }
                    yield return null;
                }

                // �ٷ� ���� ����!
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
                    Debug.Log($"[isTimeGoing] ����ð�: {elapsed:F2}�� / ��ǥ: {targetDuration}��");
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