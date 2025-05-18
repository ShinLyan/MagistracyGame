using MagistracyGame.Core;
using UnityEngine;
using UnityEngine.Events;

namespace MagistracyGame.Quiz
{
    [RequireComponent(typeof(QuizView), typeof(TextTyper))]
    public class Quiz : MonoBehaviour, IGame
    {
        [SerializeField] private QuizData _data;
        [SerializeField] private GameObject _startPanel;

        private QuizView _view;
        private TextTyper _textTyper;
        private int _questionIndex;

        private int QuestionIndex
        {
            get => _questionIndex;
            set
            {
                if (value >= _data.Questions.Length)
                {
                    FinishGame();
                    return;
                }

                _questionIndex = value;
            }
        }

        private QuizQuestion Question => _data.Questions[QuestionIndex];

        private void Awake()
        {
            _view = GetComponent<QuizView>();
            _textTyper = GetComponent<TextTyper>();
        }

        private void Start()
        {
            _startPanel.SetActive(true);
            ShowNextQuestion();
        }

        private void ShowNextQuestion()
        {
            _view.ShowQuestion(Question, HandleAnswer);
            _view.UpdateQuestionCounter(QuestionIndex + 1, _data.Questions.Length);
        }

        private void HandleAnswer(int selectedAnswerIndex)
        {
            if (IsGameFinished) return;

            bool isCorrect = selectedAnswerIndex == Question.CorrectAnswerIndex;
            _view.ShowAnswerFeedback(selectedAnswerIndex, Question.CorrectAnswerIndex, isCorrect);

            string guideText = isCorrect ? Question.GuideTextCorrect : Question.GuideTextIncorrect;
            _view.ShowGuidePanel(guideText, OnNextQuestionClicked);
            _textTyper.TypeText(guideText, 100f);
        }

        private void OnNextQuestionClicked()
        {
            _view.HideGuidePanel();
            QuestionIndex++;
            ShowNextQuestion();
        }

        #region IGame

        public bool IsGameFinished { get; private set; }

        [field: SerializeField] public UnityEvent OnGameFinished { get; private set; }

        public void FinishGame()
        {
            IsGameFinished = true;
            OnGameFinished?.Invoke();
        }

        #endregion
    }
}