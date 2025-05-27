using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CongratsMenu : MonoBehaviour
{
    [SerializeField] private Vector2 characterStartPos = new Vector2(700, 0);
    [SerializeField] private Vector2 textStartPos = new Vector2(-1320, 60);
    [SerializeField] private Vector2 characterEndPos = new Vector2(0, 0);
    [SerializeField] private Vector2 textEndPos = new Vector2(80, 60);
    [SerializeField] private RectTransform _characterImage;
    [SerializeField] private RectTransform _dialogueText;
    [SerializeField] private Image _winPanel;

    private float animationDuration = 1.0f;

       private void Start()
    {
        StartCoroutine(AnimateIn());
    }

    private IEnumerator AnimateIn()
    {       
        float elapsedTime = 0f;

        Vector2 characterStart = characterStartPos;
        Vector2 textStart = textStartPos;
        _characterImage.anchoredPosition = characterStart;
        _dialogueText.anchoredPosition = textStart;

        StartCoroutine(FadeTo(0.5f));
        yield return new WaitForSeconds(1.0f);

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            _characterImage.anchoredPosition = Vector2.Lerp(characterStart, characterEndPos, Mathf.SmoothStep(0, 1, t));
            _dialogueText.anchoredPosition = Vector2.Lerp(textStart, textEndPos, Mathf.SmoothStep(0, 1, t));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _characterImage.anchoredPosition = characterEndPos;
        _dialogueText.anchoredPosition = textEndPos;
    }

    private IEnumerator AnimateOut(Action onComplete)
    {
        float elapsedTime = 0f;

        Vector2 characterStart = _characterImage.anchoredPosition;
        Vector2 textStart = _dialogueText.anchoredPosition;

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            _characterImage.anchoredPosition = Vector2.Lerp(characterStart, characterStartPos, Mathf.SmoothStep(0, 1, t));
            _dialogueText.anchoredPosition = Vector2.Lerp(textStart, textStartPos, Mathf.SmoothStep(0, 1, t));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _characterImage.anchoredPosition = characterStartPos;
        _dialogueText.anchoredPosition = textStartPos;        

        StartCoroutine(FadeTo(1f));
        yield return new WaitForSeconds(1.51f);

        onComplete?.Invoke();
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        Color startColor = _winPanel.color;
        float startAlpha = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / (animationDuration);
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, Mathf.SmoothStep(0, 1, t));
            _winPanel.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _winPanel.color = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
    }
}
