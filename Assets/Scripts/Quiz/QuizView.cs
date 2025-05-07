using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MagistracyGame.Quiz
{
    public class QuizView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _questionText;
        [SerializeField] private TextMeshProUGUI _questionCounterText;
        [SerializeField] private Button[] _answerButtons;

        [Header("Guide")]
        [SerializeField] private GameObject _guidePanel;
        [SerializeField] private TextMeshProUGUI _guideText;
        [SerializeField] private Button _nextQuestionButton;

        [Header("Outline Colors")]
        [SerializeField] private Color _correctColor = new(0, 1, 0, 1);
        [SerializeField] private Color _wrongColor = new(1, 0, 0, 1);
        [SerializeField] private Color _defaultOutline = new(0, 0, 0, 0);

        [Header("Background Colors")]
        [SerializeField] private Color _correctBackgroundColor = new(0.8f, 1, 0.75f, 1);
        [SerializeField] private Color _wrongBackgroundColor = new(1, 0.75f, 0.75f, 1);
        [SerializeField] private Color _defaultBackgroundColor = new(1, 1, 1, 1);

        private void Start() => HideGuidePanel();

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _guidePanel.activeInHierarchy)
            {
                if (!IsPointerOverButton(_nextQuestionButton))
                {
                    if (_guideText != null && TryGetComponent<TextTyper>(out var textTyper))
                    {
                        textTyper.CompleteTextImmediately();
                    }
                }
            }
        }

        private bool IsPointerOverButton(Button button)
        {
            if (button == null || !button.gameObject.activeInHierarchy)
                return false;

            var pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            var raycastResults = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, raycastResults);

            foreach (var result in raycastResults)
            {
                if (result.gameObject == button.gameObject || result.gameObject.transform.IsChildOf(button.transform))
                {
                    return true;
                }
            }
            return false;
        }

        public void UpdateQuestionCounter(int current, int total)
        {
            _questionCounterText.text = $"Вопрос: {current} из {total}";
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
            SetButtonBackground(_answerButtons[selectedIndex], isCorrect ? _correctBackgroundColor : _wrongBackgroundColor);

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
            if (button.TryGetComponent<Image>(out var image))
            {
                image.color = color;
            }
        }

        private static void SetButtonText(Button button, string text)
        {
            var label = button.GetComponentInChildren<TextMeshProUGUI>();
            if (label) label.text = text;
        }

        public void ShowGuidePanel(string text, Action onClick)
        {
            _guidePanel.SetActive(true);
            _guideText.text = text;

            _nextQuestionButton.gameObject.SetActive(true);
            _nextQuestionButton.onClick.RemoveAllListeners();
            _nextQuestionButton.onClick.AddListener(() =>
            {
                onClick?.Invoke();
            });
        }

        public void HideGuidePanel()
        {
            _guidePanel.SetActive(false);
            _nextQuestionButton.gameObject.SetActive(false);
            _nextQuestionButton.onClick.RemoveAllListeners();
        }
    }
}