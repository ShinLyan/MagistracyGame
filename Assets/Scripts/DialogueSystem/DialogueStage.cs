using UnityEngine;
using UnityEngine.UI;

public class DialogueStage : MonoBehaviour
{
    [SerializeField] private GameObject _blackBackgroundGameField;
    [SerializeField] private Image _blackBackground;
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _studentCardStage;
    [SerializeField] private GameObject _fillwords;
    [SerializeField] private GameObject _firstQuiz;
    [SerializeField] private GameObject _magoLego;
    [SerializeField] private GameObject _mergeGame;
    [SerializeField] private GameObject _secondQuiz;
    [SerializeField] private GameObject _puzzle;
    [SerializeField] private GameObject _diplomStage;

    private int levelIndex;

    private void Start() => InitializeDialogStage();

    private void InitializeDialogStage()
    {
        _blackBackground.enabled = true;
        levelIndex = 0;

        var screens = new[]
        {
            _studentCardStage,
            _fillwords,
            _firstQuiz,
            _mergeGame,
            _secondQuiz,
            _puzzle,
            _magoLego,
            _diplomStage
        };
        foreach (var screen in screens) screen.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void StartDialogue()
    {
        switch (levelIndex)
        {
            case 0:
            {
                _dialogueManager.SwitchDialoguePanel(true);
                _blackBackgroundGameField.SetActive(true);
                _mainMenu.SetActive(false);
                break;
            }
            case 1:
            {
                _dialogueManager.SwitchDialoguePanel(true);
                _blackBackgroundGameField.SetActive(true);
                _studentCardStage.SetActive(false);
                break;
            }
            case 2:
            {
                _dialogueManager.SwitchDialoguePanel(true);
                _blackBackgroundGameField.SetActive(true);
                _fillwords.SetActive(false);
                break;
            }
            case 3:
            {
                _dialogueManager.SwitchDialoguePanel(true);
                _blackBackgroundGameField.SetActive(true);
                _magoLego.SetActive(false);
                break;
            }
            case 4:
            {
                _dialogueManager.SwitchDialoguePanel(true);
                _blackBackgroundGameField.SetActive(true);
                _firstQuiz.SetActive(false);
                break;
            }
            case 5:
            {
                _dialogueManager.SwitchDialoguePanel(true);
                _blackBackgroundGameField.SetActive(true);
                _mergeGame.SetActive(false);
                break;
            }
            case 6:
            {
                _dialogueManager.SwitchDialoguePanel(true);
                _blackBackgroundGameField.SetActive(true);
                _secondQuiz.SetActive(false);
                break;
            }
            case 7:
            {
                _dialogueManager.SwitchDialoguePanel(true);
                _blackBackgroundGameField.SetActive(true);
                _puzzle.SetActive(false);
                break;
            }
        }

        _dialogueManager.ContinueDialogue();
    }

    public void EndDialogue()
    {
        switch (levelIndex)
        {
            case 0:
            {
                _dialogueManager.SwitchDialoguePanel(false);
                _blackBackgroundGameField.SetActive(false);
                _studentCardStage.SetActive(true);
                break;
            }
            case 1:
            {
                _dialogueManager.SwitchDialoguePanel(false);
                _blackBackgroundGameField.SetActive(false);
                _fillwords.SetActive(true);
                break;
            }

            case 2:
            {
                _dialogueManager.SwitchDialoguePanel(false);
                _blackBackgroundGameField.SetActive(false);
                _magoLego.SetActive(true);
                break;
            }
            case 3:
            {
                _dialogueManager.SwitchDialoguePanel(false);
                _blackBackgroundGameField.SetActive(false);
                _firstQuiz.SetActive(true);
                break;
            }
            case 4:
            {
                _dialogueManager.SwitchDialoguePanel(false);
                _blackBackgroundGameField.SetActive(false);
                _mergeGame.SetActive(true);
                break;
            }
            case 5:
            {
                _dialogueManager.SwitchDialoguePanel(false);
                _blackBackgroundGameField.SetActive(false);
                _secondQuiz.SetActive(true);
                break;
            }
            case 6:
            {
                _dialogueManager.SwitchDialoguePanel(false);
                _blackBackgroundGameField.SetActive(false);
                _puzzle.SetActive(true);
                break;
            }
            case 7:
            {
                _dialogueManager.SwitchDialoguePanel(false);
                _blackBackgroundGameField.SetActive(false);
                _diplomStage.SetActive(true);
                break;
            }
        }

        levelIndex++;
    }
}