using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CongratsMenu : MonoBehaviour
{
    [SerializeField] private Vector2 characterStartPos = new(700, 0);
    [SerializeField] private Vector2 textStartPos = new(-1320, 60);
    [SerializeField] private Vector2 characterEndPos = new(0, 0);
    [SerializeField] private Vector2 textEndPos = new(80, 60);
    [SerializeField] private RectTransform _characterImage;
    [SerializeField] private RectTransform _dialogueText;
    [SerializeField] private Image _winPanel;

    private const float AnimationDuration = 0.5f;

    private void Start()
    {
        StartCoroutine(AnimateIn());
    }

    private IEnumerator AnimateIn()
    {
        float elapsedTime = 0f;

        var characterStart = characterStartPos;
        var textStart = textStartPos;
        _characterImage.anchoredPosition = characterStart;
        _dialogueText.anchoredPosition = textStart;

        StartCoroutine(FadeTo(0.5f));
        yield return new WaitForSeconds(1.0f);

        while (elapsedTime < AnimationDuration)
        {
            float t = elapsedTime / AnimationDuration;
            _characterImage.anchoredPosition = Vector2.Lerp(characterStart, characterEndPos, Mathf.SmoothStep(0, 1, t));
            _dialogueText.anchoredPosition = Vector2.Lerp(textStart, textEndPos, Mathf.SmoothStep(0, 1, t));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _characterImage.anchoredPosition = characterEndPos;
        _dialogueText.anchoredPosition = textEndPos;
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        var startColor = _winPanel.color;
        float startAlpha = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < AnimationDuration)
        {
            float t = elapsedTime / AnimationDuration;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, Mathf.SmoothStep(0, 1, t));
            _winPanel.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _winPanel.color = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
    }
}