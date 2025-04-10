using UnityEngine;
using UnityEngine.UI;

public class DialogueStage : MonoBehaviour
{
    [SerializeField] private GameObject _blackBackgroundGameField;
    [SerializeField] private Image _blackBackground;
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _studentCardStage;
    private int levelIndex;

    private void Start() => InitializeDialogStage();

    private void InitializeDialogStage()
    {
        _blackBackground.enabled = true;
        levelIndex = 0;
    }

    public void StartDialogue()
    {
        switch (levelIndex)
        {
            case 0:
            {
                _mainMenu.SetActive(false);
                _dialogueManager.SwitchDialoguePanel(true);
                _blackBackgroundGameField.SetActive(true);
                break;
            }
            case 1:
            {
                _dialogueManager.SwitchDialoguePanel(true);
                _blackBackgroundGameField.SetActive(true);
                _studentCardStage.SetActive(false);
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
        }

        levelIndex++;
    }

    private void AddToBlackBackground(Transform transform, bool worldPositionStays = true)
    {
        transform.SetParent(_blackBackgroundGameField.GetComponent<Transform>(), worldPositionStays);
    }

    private static void RemoveFromBlackBackground(Transform transform, Transform place, bool worldPositionStays = true)
    {
        transform.SetParent(place, worldPositionStays);
    }
}