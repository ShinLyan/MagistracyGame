﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MagistracyGame.Quiz
{
    [RequireComponent(typeof(TextTyper))]
    public class QuizView : MonoBehaviour
    {
        [Header("Question")]
        [SerializeField] private TextMeshProUGUI _questionText;

        [SerializeField] private TextMeshProUGUI _questionCounterText;
        [SerializeField] private Button[] _answerButtons;

        [Header("Guide")]
        [SerializeField] private Button _taskPanelButton;

        [SerializeField] private TextMeshProUGUI _guideText;
        [SerializeField] private Button _nextQuestionButton;

        [Header("Outline Colors")]
        [SerializeField] private Color _correctColor;

        [SerializeField] private Color _wrongColor;
        [SerializeField] private Color _defaultOutline;

        [Header("Background Colors")]
        [SerializeField] private Color _correctBackgroundColor;

        [SerializeField] private Color _wrongBackgroundColor;
        [SerializeField] private Color _defaultBackgroundColor;

        private void Awake() => _taskPanelButton.onClick.AddListener(HandleGuidePanelClick);

        private void Start() => HideGuidePanel();

        private void HandleGuidePanelClick()
        {
            var textTyper = GetComponent<TextTyper>();
            textTyper.CompleteTextImmediately();
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
                SetButtonBackground(button, _defaultBackgroundColor);

                int index = i;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => onAnswerSelected(index));
            }
        }

        public void ShowAnswerFeedback(int selectedIndex, int correctIndex, bool isCorrect)
        {
            DisableAnswerButtons();

            SetOutline(_answerButtons[selectedIndex], isCorrect ? _correctColor : _wrongColor);
            SetButtonBackground(_answerButtons[selectedIndex],
                isCorrect ? _correctBackgroundColor : _wrongBackgroundColor);

            if (!isCorrect)
            {
                SetOutline(_answerButtons[correctIndex], _correctColor);
                SetButtonBackground(_answerButtons[correctIndex], _correctBackgroundColor);
            }
        }

        private void DisableAnswerButtons()
        {
            foreach (var button in _answerButtons) button.interactable = false;
        }

        private static void SetOutline(Button button, Color color)
        {
            if (!button.TryGetComponent<Outline>(out var outline)) return;

            outline.effectColor = color;
            outline.effectDistance = color.a > 0 ? new Vector2(5, 5) : Vector2.zero;
        }

        private static void SetButtonBackground(Button button, Color color)
        {
            if (button.TryGetComponent<Image>(out var image)) image.color = color;
        }

        private static void SetButtonText(Button button, string text)
        {
            var label = button.GetComponentInChildren<TextMeshProUGUI>();
            if (label) label.text = text;
        }

        public void UpdateQuestionCounter(int current, int total)
        {
            _questionCounterText.text = $"Вопрос: {current} из {total}";
        }

        public void ShowGuidePanel(string text, Action onClick)
        {
            _taskPanelButton.gameObject.SetActive(true);
            _guideText.text = text;
            _nextQuestionButton.onClick.RemoveAllListeners();
            _nextQuestionButton.onClick.AddListener(() => { onClick?.Invoke(); });
        }

        public void HideGuidePanel()
        {
            _taskPanelButton.gameObject.SetActive(false);
            _nextQuestionButton.onClick.RemoveAllListeners();
        }
    }
}