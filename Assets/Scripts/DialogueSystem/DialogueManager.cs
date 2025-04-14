using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextAsset _dialogueXml;
    [SerializeField] private Button[] _dialoguePanels;
    [SerializeField] private TMP_Text _dialoguePanelText;
    [SerializeField] private DialogueStage _dialogueStage;
    private Dialogue dialogue;
    private int phraseIndex;

    private void Start()
    {
        phraseIndex = 0;
        _dialoguePanelText.text = string.Empty;
        dialogue = Dialogue.Load(_dialogueXml);
    }

    public void ContinueDialogue()
    {
        StartCoroutine(WriteSentence(dialogue.Nodes[phraseIndex].Text));
    }

    private IEnumerator WriteSentence(string sentence)
    {
        const float TextDelay = 0.03f;
        _dialoguePanelText.text = string.Empty;

        foreach (char symbol in sentence)
        {
            _dialoguePanelText.text += symbol;
            yield return new WaitForSeconds(TextDelay);
        }
    }

    public void OnClickBackButton()
    {
        if (phraseIndex == 0 || dialogue.Nodes[phraseIndex - 1].IsEnd) return;

        StopAllCoroutines();
        phraseIndex -= 2;
        NextSentence();
    }

    public void OnClickDialogue()
    {
        if (_dialoguePanelText.text == dialogue.Nodes[phraseIndex].Text)
        {
            
            if (dialogue.Nodes[phraseIndex].IsEnd) _dialogueStage.EndDialogue();
            NextSentence();
        }
        else
        {
            StopAllCoroutines();
            _dialoguePanelText.text = dialogue.Nodes[phraseIndex].Text;
        }
    }

    private void NextSentence()
    {
        if (phraseIndex >= dialogue.Nodes.Length - 1) return;

        phraseIndex++;
        StartCoroutine(WriteSentence(dialogue.Nodes[phraseIndex].Text));
    }

    public void SwitchDialoguePanel(bool isEnabled)
    {
        foreach (var dialoguePanel in _dialoguePanels) dialoguePanel.enabled = isEnabled;
    }

    public void SkipSentence()
    {
        StopAllCoroutines();
        phraseIndex++;
    }
}