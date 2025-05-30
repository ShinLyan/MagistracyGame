using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueStage : MonoBehaviour
{
    [SerializeField] private GameObject _dialogueWindow;
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _studentCardStage;
    [SerializeField] private GameObject _fillwords;
    [SerializeField] private GameObject _magoLego;
    [SerializeField] private GameObject _firstQuiz;
    [SerializeField] private GameObject _mergeGame;
    [SerializeField] private GameObject _secondQuiz;
    [SerializeField] private GameObject _puzzle;
    [SerializeField] private GameObject _diplomStage;
    [SerializeField] private Image _blackBackground;

    private int levelIndex;

    [SerializeField] private RectTransform _characterImage;
    [SerializeField] private RectTransform _dialogueText;
    [SerializeField] private GameObject[] _stages;

    private const float AnimationDuration = 0.3f;
    [SerializeField] private Vector2 characterStartPos = new(570, 0);
    [SerializeField] private Vector2 textStartPos = new(-1350, 50);
    [SerializeField] private Vector2 characterEndPos = new(-80, 0);
    [SerializeField] private Vector2 textEndPos = new(80, 50);

    private void Start() => InitializeDialogStage();

    private void InitializeDialogStage()
    {
        levelIndex = 0;

        var screens = new[]
        {
            _dialogueWindow,
            _studentCardStage,
            _fillwords,
            _magoLego,
            _firstQuiz,
            _mergeGame,
            _secondQuiz,
            _puzzle,
            _diplomStage
        };
        foreach (var screen in screens) screen.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void StartDialogue()
    {
        var screens = new[]
        {
            _studentCardStage,
            _fillwords,
            _magoLego,
            _firstQuiz,
            _mergeGame,
            _secondQuiz,
            _puzzle,
            _diplomStage
        };

        StopAllCoroutines();
        _dialogueWindow.GetComponent<Button>().enabled = false;
        StartCoroutine(AnimateIn(screens[levelIndex], () =>
        {
            _dialogueManager.ContinueDialogue();
            _dialogueWindow.GetComponent<Button>().enabled = true;
        }));
    }

    public void EndDialogue()
    {
        var screens = new[]
        {
            _studentCardStage,
            _fillwords,
            _magoLego,
            _firstQuiz,
            _mergeGame,
            _secondQuiz,
            _puzzle,
            _diplomStage,
            _mainMenu
        };
        _dialogueWindow.GetComponent<Button>().enabled = false;
        StartCoroutine(AnimateOut(() =>
        {
            StartCoroutine(FadeTo(0f));
            _dialogueWindow.SetActive(false);

            foreach (var screen in screens) screen.SetActive(false);
            screens[levelIndex].SetActive(true);

            levelIndex++;
        }));
    }

    private IEnumerator AnimateIn(GameObject screen, Action onComplete)
    {
        _dialogueText.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "";
        float elapsedTime = 0f;

        var characterStart = characterStartPos;
        var textStart = textStartPos;
        _characterImage.anchoredPosition = characterStart;
        _dialogueText.anchoredPosition = textStart;

        StartCoroutine(FadeTo(1f));
        yield return new WaitForSeconds(1f);

        screen.SetActive(false);
        _dialogueWindow.SetActive(true);


        StartCoroutine(FadeTo(0f));
        yield return new WaitForSeconds(1f);

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

        onComplete?.Invoke();
    }

    private IEnumerator AnimateOut(Action onComplete)
    {
        float elapsedTime = 0f;

        var characterStart = _characterImage.anchoredPosition;
        var textStart = _dialogueText.anchoredPosition;

        while (elapsedTime < AnimationDuration)
        {
            float t = elapsedTime / AnimationDuration;
            _characterImage.anchoredPosition =
                Vector2.Lerp(characterStart, characterStartPos, Mathf.SmoothStep(0, 1, t));
            _dialogueText.anchoredPosition = Vector2.Lerp(textStart, textStartPos, Mathf.SmoothStep(0, 1, t));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _characterImage.anchoredPosition = characterStartPos;
        _dialogueText.anchoredPosition = textStartPos;
        _dialogueText.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "";


        StartCoroutine(FadeTo(1f));
        yield return new WaitForSeconds(1.51f);

        onComplete?.Invoke();
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        var startColor = _blackBackground.color;
        float startAlpha = startColor.a;
        float elapsedTime = 0f;
        while (elapsedTime < AnimationDuration * 3)
        {
            float t = elapsedTime / (AnimationDuration * 3);
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, Mathf.SmoothStep(0, 1, t));
            _blackBackground.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _blackBackground.color = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
    }
}