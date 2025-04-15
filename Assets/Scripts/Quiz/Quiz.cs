using UnityEngine;

namespace MagistracyGame.Quiz
{
    public class Quiz : MonoBehaviour
    {
        [SerializeField] private QuizData _data;
        [SerializeField] private QuizView _view;
        [SerializeField] private TextTyper _guideTyper;

        private bool _isGameCompleted;
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

        private void Start() => ShowQuestion();

        private void ShowQuestion()
        {
            _view.ShowQuestion(Question, HandleAnswer);
            _view.UpdateQuestionCounter(QuestionIndex + 1, _data.Questions.Length);
        }

        private void HandleAnswer(int selectedAnswerIndex)
        {
            if (_isGameCompleted) return;

            bool isCorrect = selectedAnswerIndex == Question.CorrectAnswerIndex;
            _view.ShowAnswerFeedback(selectedAnswerIndex, Question.CorrectAnswerIndex, isCorrect);

            string guideText = isCorrect ? Question.GuideTextCorrect : Question.GuideTextIncorrect;
            _view.ShowGuidePanel(guideText, OnNextQuestionClicked);
            _guideTyper.TypeText(guideText, 10f);
        }

        private void OnNextQuestionClicked()
        {
            _view.HideGuidePanel();
            QuestionIndex++;
            ShowQuestion();
        }

        private void FinishGame()
        {
            _isGameCompleted = true;
            Debug.Log("Квиз завершён!");
        }
    }
}