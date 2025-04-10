using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    /// <summary> ����� �������� - XML ����.</summary>
    [SerializeField] private TextAsset _dialogueXml;

    /// <summary> ���������� ������.</summary>
    [SerializeField] private Button[] _dialoguePanels;

    /// <summary> ����� ���������� ������.</summary>
    [SerializeField] private TMP_Text _dialoguePanelText;

    /// <summary> ���� ������.</summary>
    [SerializeField] private DialogStage _dialogStage;

    /// <summary> ������, ��������� �� XML �����.</summary>
    private Dialogue dialogue;

    /// <summary> ������ ����� � �������.</summary>
    private int phraseIndex;

    /// <summary> �������� ��� ��������� ������� � ������.</summary>
    private const float TextDelay = 0.03f;

    /// <summary> ������ ��������.</summary>
    /// <remarks> ���������� ����� ��������������� ������, ������������� �������� ������� ��������.</remarks>
    private void Start()
    {
        phraseIndex = 0;

        // ������� ���������� ������.
        _dialoguePanelText.text = string.Empty;

        // ��������� ������ �� XML �����. 
        TextAsset localizedTextAsset = _dialogueXml;
        dialogue = Dialogue.Load(localizedTextAsset);
    }

    /// <summary> ���������� ������.</summary>
    public void ContinueDialogue()
    {
        StartCoroutine(WriteSentence(dialogue.Nodes[phraseIndex].Text));
    }

    /// <summary> ��������, ������� ����� ����������� �� ���������� ������.</summary>
    /// <remarks> �������� ����� �� ������ ������� � ����������� ���������.</remarks>
    /// <returns> ���������� �����������, ������� ������������ �� ���������� ������.</returns>
    public IEnumerator WriteSentence(string sentence)
    {
        // ������� ���������� ������.
        _dialoguePanelText.text = string.Empty;

        // ��������� ������� � ���������.
        foreach (char symbol in sentence)
        {
            _dialoguePanelText.text += symbol;
            yield return new WaitForSeconds(TextDelay);
        }
    }

    public void OnClickBackButton()
    {
        if (phraseIndex != 0 && !dialogue.Nodes[phraseIndex - 1].IsEnd)
        {
            StopAllCoroutines();
            phraseIndex -= 2;
            NextSentence();
        }
    }

    /// <summary> ������� �� ������� ������ ���������� ������.</summary>
    public void OnClickDialogue()
    {
        // ���� ����� ���������� �� ������ ���������.
        if (_dialoguePanelText.text == dialogue.Nodes[phraseIndex].Text)
        {
            // ���� ����� �����������, �� ����������� ������.
            if (dialogue.Nodes[phraseIndex].IsEnd)
            {
                NextSentence();
                _dialogStage.EndDialogue();
            }
            else // ����� ����������.
            {
                NextSentence();
            }
        }
        else
        {
            StopAllCoroutines();

            // ������� ����� ���� �����.
            _dialoguePanelText.text = dialogue.Nodes[phraseIndex].Text;
        }
    }

    /// <summary> ��������� �����������.</summary>
    private void NextSentence()
    {
        if (phraseIndex < dialogue.Nodes.Length - 1)
        {
            phraseIndex++;
            StartCoroutine(WriteSentence(dialogue.Nodes[phraseIndex].Text));
        }

    }

    /// <summary> ���������� ���������� �����.</summary>
    /// <param name="isEnabled"></param>
    public void SwitchDialoguePanel(bool isEnabled)
    {
        foreach (var dialoguePanel in _dialoguePanels)
        {
            dialoguePanel.enabled = isEnabled;
        }
    }

    /// <summary> ���������� �����������.</summary>
    public void SkipSentence()
    {
        StopAllCoroutines();

        phraseIndex++;
    }
}
