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
    public Vector2 panelOffset;
    public GameObject contents;

    private char cursor_char = '|';

    private RectTransform panelRect;
    private Vector2 originalPanelPos;
    public List<DialogueScene> scenes;
    private void Awake()
    {
        panelRect = TextPanel.GetComponent<RectTransform>();
        originalPanelPos = panelRect.anchoredPosition;
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

                    if (scene.isFirstAppearance)
                        StandingImage[0].rectTransform.localScale = Vector3.one;
                    else
                        StandingImage[0].rectTransform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
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

                    yield return new WaitForSeconds(2.2f);

                    if (TextPanel != null)
                        TextPanel.SetActive(true);
                }
            }

            // �ؽ�Ʈ ǥ�� (Ÿ���� ȿ��)
            TestTexts[idx].gameObject.SetActive(true);
            string full = scene.text;
            int len = full.GetTypingLength();

            for (int i = 0; i <= len; i++)
            {
                TestTexts[idx].text = full.Typing(i);
                yield return new WaitForSeconds(0.025f);
            }

            // Ŀ�� ������
            float blink = 0f;
            bool cursor = false;
            while (!Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Return))
            {
                blink += Time.deltaTime;
                if (blink >= 0.5f)
                {
                    blink = 0f;
                    cursor = !cursor;
                }
                TestTexts[idx].text = full + (cursor ? cursor_char.ToString() : "");
                yield return null;
            }
            TestTexts[idx].text = full;

            yield return new WaitForSeconds(scene.postDelay);
        }
        contents.SetActive(false);
    }


}