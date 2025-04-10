using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    /// <summary> Текст диалогов - XML файл.</summary>
    [SerializeField] private TextAsset _dialogueXml;

    /// <summary> Диалоговая панель.</summary>
    [SerializeField] private Button[] _dialoguePanels;

    /// <summary> Текст диалоговой панели.</summary>
    [SerializeField] private TMP_Text _dialoguePanelText;

    /// <summary> Этап диалог.</summary>
    [SerializeField] private DialogStage _dialogStage;

    /// <summary> Диалог, считанный из XML файла.</summary>
    private Dialogue dialogue;

    /// <summary> Индекс фразы в диалоге.</summary>
    private int phraseIndex;

    /// <summary> Задержка при вылетании символа в тексте.</summary>
    private const float TextDelay = 0.03f;

    /// <summary> Запуск обучения.</summary>
    /// <remarks> Происходит выбор локализованного текста, инициализация обучения нулевым индексом.</remarks>
    private void Start()
    {
        phraseIndex = 0;

        // Очищаем диалоговую панель.
        _dialoguePanelText.text = string.Empty;

        // Загружаем данные из XML файла. 
        TextAsset localizedTextAsset = _dialogueXml;
        dialogue = Dialogue.Load(localizedTextAsset);
    }

    /// <summary> Продолжить диалог.</summary>
    public void ContinueDialogue()
    {
        StartCoroutine(WriteSentence(dialogue.Nodes[phraseIndex].Text));
    }

    /// <summary> Корутина, которая пишет предложение на диалоговой панели.</summary>
    /// <remarks> Корутина выдаёт по одному символу с определённой задержкой.</remarks>
    /// <returns> Возвращает предложение, которое отображается на диалоговой панели.</returns>
    public IEnumerator WriteSentence(string sentence)
    {
        // Очищаем диалоговую панель.
        _dialoguePanelText.text = string.Empty;

        // Добавляем символы с задержкой.
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

    /// <summary> Событие на нажатие кнопки диалоговой панели.</summary>
    public void OnClickDialogue()
    {
        // Если фраза отображена на панели полностью.
        if (_dialoguePanelText.text == dialogue.Nodes[phraseIndex].Text)
        {
            // Если фраза завершающая, то заканчиваем диалог.
            if (dialogue.Nodes[phraseIndex].IsEnd)
            {
                NextSentence();
                _dialogStage.EndDialogue();
            }
            else // Иначе продолжаем.
            {
                NextSentence();
            }
        }
        else
        {
            StopAllCoroutines();

            // Выдаваём сразу весь текст.
            _dialoguePanelText.text = dialogue.Nodes[phraseIndex].Text;
        }
    }

    /// <summary> Следующее предложение.</summary>
    private void NextSentence()
    {
        if (phraseIndex < dialogue.Nodes.Length - 1)
        {
            phraseIndex++;
            StartCoroutine(WriteSentence(dialogue.Nodes[phraseIndex].Text));
        }

    }

    /// <summary> Управление Диалоговым окном.</summary>
    /// <param name="isEnabled"></param>
    public void SwitchDialoguePanel(bool isEnabled)
    {
        foreach (var dialoguePanel in _dialoguePanels)
        {
            dialoguePanel.enabled = isEnabled;
        }
    }

    /// <summary> Пропустить предложение.</summary>
    public void SkipSentence()
    {
        StopAllCoroutines();

        phraseIndex++;
    }
}
