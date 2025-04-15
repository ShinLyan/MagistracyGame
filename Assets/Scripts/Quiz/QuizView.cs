using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MagistracyGame.Quiz
{
    public class QuizView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _questionText;
        [SerializeField] private TextMeshProUGUI _questionCounterText;
        [SerializeField] private Button[] _answerButtons;

        [Header("Guide")]
        [SerializeField] private Button _guidePanelButton;

        [SerializeField] private TextMeshProUGUI _guideText;

        [Header("Outline Colors")]
        [SerializeField] private Color _correctColor = new(0, 1, 0, 1);

        [SerializeField] private Color _wrongColor = new(1, 0, 0, 1);
        [SerializeField] private Color _defaultOutline = new(0, 0, 0, 0);

        private void Start() => HideGuidePanel();

        public void UpdateQuestionCounter(int current, int total)
        {
            _questionCounterText.text = $"{current} / {total}";
        }

        public void ShowQuestion(QuizQuestion question, Action<int> onAnswerSelected)
        {
            _questionText.text = question.QuestionText;
            for (int i = 0; i < _answerButtons.Length; i++)
            {
                var button = _answerButtons[i];
                button.interactable = true;
                SetButtonText(button, question.Answers[i]);
                SetOutline(button, _defaultOutline);

                int index = i;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => onAnswerSelected(index));
            }
        }

        public void ShowAnswerFeedback(int selectedIndex, int correctIndex, bool isCorrect)
        {
            DisableAnswerButtons();
            SetOutline(_answerButtons[selectedIndex], isCorrect ? _correctColor : _wrongColor);

            if (!isCorrect)
                SetOutline(_answerButtons[correctIndex], _correctColor);
        }

        private void DisableAnswerButtons()
        {
            foreach (var button in _answerButtons) button.interactable = false;
        }

        private static void SetOutline(Button button, Color color)
        {
            if (!button.TryGetComponent<Outline>(out var outline)) return;

            outline.effectColor = color;
        }

        private static void SetButtonText(Button button, string text)
        {
            var label = button.GetComponentInChildren<TextMeshProUGUI>();
            if (label) label.text = text;
        }

        public void ShowGuidePanel(string text, Action onClick)
        {
            _guidePanelButton.gameObject.SetActive(true);
            _guideText.text = text;

            _guidePanelButton.onClick.RemoveAllListeners();
            _guidePanelButton.onClick.AddListener(() => onClick?.Invoke());
        }

        public void HideGuidePanel()
        {
            _guidePanelButton.gameObject.SetActive(false);
            _guidePanelButton.onClick.RemoveAllListeners();
        }
    }
}