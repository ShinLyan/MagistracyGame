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

    private int levelIndex;

    [SerializeField] private RectTransform _characterImage;
    [SerializeField] private RectTransform _dialogueText;
    [SerializeField] private GameObject[] _stages;

    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private Vector2 characterStartPos = new Vector2(570, 0);
    [SerializeField] private Vector2 textStartPos = new Vector2(-1350, 50);
    [SerializeField] private Vector2 characterEndPos = new Vector2(-80, 0);
    [SerializeField] private Vector2 textEndPos = new Vector2(80, 50);
    

    private bool isAnimating = false;

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
        _dialogueWindow.SetActive(true);
        screens[levelIndex].SetActive(false);

        StopAllCoroutines();
        _dialogueWindow.GetComponent<Button>().enabled = false;
        StartCoroutine(AnimateIn(() =>
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
            _diplomStage
        };
        _dialogueWindow.GetComponent<Button>().enabled = false;
        StartCoroutine(AnimateOut(() =>
        {
            _dialogueWindow.SetActive(false);
            screens[levelIndex].SetActive(true);

            levelIndex++;
        }));
    }

    private IEnumerator AnimateIn(Action onComplete)
    {
        _dialogueText.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "";
        float elapsedTime = 0f;

        Vector2 characterStart = characterStartPos;
        Vector2 textStart = textStartPos;

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

        onComplete?.Invoke();
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
        _dialogueText.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "";
        
        onComplete?.Invoke();
    }
}