using UnityEngine;
using UnityEngine.UI;

public class DialogStage : MonoBehaviour
{
    /// <summary> ׸���� ��� �������� ����.</summary>
    [SerializeField] private GameObject __blackBackgroundGameField;

    /// <summary> ׸���� ���.</summary> 
    [SerializeField] private Image _blackBackground;

    [SerializeField] private DialogManager _dialogManager;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _studentCardStage;

    /// <summary> ������ ����� � �������.</summary>
    private int levelIndex;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /// <summary>��� ������ �������������� ��������.</summary>
    private void Start() => InitializeDialogStage();

    /// <summary> ���������������� �����������.</summary>
    private void InitializeDialogStage()
    {
        _blackBackground.enabled = true;
        levelIndex = 0;

    }

    /// <summary> ������ ������.</summary>
    public void StartDialogue()
    {
        switch (levelIndex)
        {
            case 0:
                {
                    _mainMenu.SetActive(false);
                    _dialogManager.SwitchDialoguePanel(true);
                    __blackBackgroundGameField.SetActive(true);
                    break;
                }
            case 1: 
                {
                    _dialogManager.SwitchDialoguePanel(true);
                    __blackBackgroundGameField.SetActive(true);
                    _studentCardStage.SetActive(false);
                    break;
                }
        }
        _dialogManager.ContinueDialogue();
    }

    /// <summary> ��������� ������.</summary>
    public void EndDialogue()
    {
        switch (levelIndex)
        {
            case 0:
                {
                    _dialogManager.SwitchDialoguePanel(false);
                    __blackBackgroundGameField.SetActive(false);
                    _studentCardStage.SetActive(true);
                    break;
                }
        }
        levelIndex += 1;

    }

    /// <summary> �������� ������ �� ����� ���.</summary>
    /// <param name="transform"> ������, ������� ���������� �������� �� ����� ���.</param>
    /// <param name="worldPositionStays"> ���� True, �� ��������� ������������ �������� ����� ��, ��� � � ������� ������������.
    /// �������� �� ������� ���� ����������.</param>
    private void AddToBlackBackground(Transform transform, bool worldPositionStays = true)
    {
        transform.SetParent(__blackBackgroundGameField.GetComponent<Transform>(), worldPositionStays);
    }

    /// <summary> ������ ������ �� ������ ����.</summary>
    /// <param name="transform"> ������, ������� ���������� ������ �� ������ ����.</param>
    /// <param name="place"> �����, ���� ���������� ��������� ������.</param>
    /// <param name="worldPositionStays"> ���� True, �� ��������� ������������ �������� ����� ��, ��� � � ������� ������������.
    /// �������� �� ������� ���� ����������.</param>
    private static void RemoveFromBlackBackground(Transform transform, Transform place, bool worldPositionStays = true)
    {
        transform.SetParent(place, worldPositionStays);
    }
}
