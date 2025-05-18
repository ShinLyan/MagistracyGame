using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextAsset _dialogueXml;
    [SerializeField] private TMP_Text _dialoguePanelText;
    [SerializeField] private DialogueStage _dialogueStage;

    private Dialogue _dialogue;
    private int _phraseIndex;

    private void Start()
    {
        _phraseIndex = 0;
        _dialoguePanelText.text = string.Empty;
        _dialogue = Dialogue.Load(_dialogueXml);
    }

    public void ContinueDialogue()
    {
        StopAllCoroutines();
        StartCoroutine(WriteSentence(_dialogue.Nodes[_phraseIndex].Text));
    }

    private IEnumerator WriteSentence(string sentence)
    {
        const float TextDelay = 0.01f;
        _dialoguePanelText.text = string.Empty;

        foreach (char symbol in sentence)
        {
            _dialoguePanelText.text += symbol;
            yield return new WaitForSeconds(TextDelay);
        }
    }

    public void OnClickBackButton()
    {
        if (_phraseIndex == 0 || _dialogue.Nodes[_phraseIndex - 1].IsEnd) return;
        StopAllCoroutines();
        _phraseIndex -= 2;
        NextSentence();
    }

    public void OnClickDialogue()
    {
        if (_dialoguePanelText.text == _dialogue.Nodes[_phraseIndex].Text)
        {
            if (_dialogue.Nodes[_phraseIndex].IsEnd) _dialogueStage.EndDialogue();
            NextSentence();
        }
        else
        {
            StopAllCoroutines();
            _dialoguePanelText.text = _dialogue.Nodes[_phraseIndex].Text;
        }
    }

    private void NextSentence()
    {
        if (_phraseIndex >= _dialogue.Nodes.Length - 1) return;

        _phraseIndex++;
        StartCoroutine(WriteSentence(_dialogue.Nodes[_phraseIndex].Text));
    }

    public void SkipSentence()
    {
        StopAllCoroutines();
        _phraseIndex++;
    }
}