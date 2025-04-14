namespace QuizManager
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using QuizData;

    public class QuizManager : MonoBehaviour
    {
        [SerializeField] private QuizData _quizData;
        [SerializeField] private QuizView _quizView;
        [SerializeField] private Color _greenOutline = new Color(0, 1, 0, 1);
        [SerializeField] private Color _redOutline = new Color(1, 0, 0, 1);

        private int _currentQuestionIndex;
        private bool _isAnswered;
        private bool _isGuideTextFinished;
        private TextTyper _textTyper;
        
        private void Start()
        {
            _textTyper = _quizView.GetComponentInChildren<TextTyper>();
            LoadQuestion(_currentQuestionIndex);
            EventSystem.current.SetSelectedGameObject(null);
        }

        private void Update()
        {
            if (_isAnswered && _isGuideTextFinished &&
                (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
            {
                MoveToNextQuestion();
            }
        }

        private void LoadQuestion(int questionIndex)
        {
            if (questionIndex >= _quizData.questions.Length)
            {
                EndQuiz();
                return;
            }

            _isAnswered = false;
            _isGuideTextFinished = false;

            _quizView.DisplayQuestion(_quizData.questions[questionIndex]);

            Button[] answerButtons = _quizView.GetAnswerButtons();
            for (int i = 0; i < answerButtons.Length; i++)
            {
                int answerIndex = i;
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => OnAnswerClicked(answerIndex));
            }
        }

        private void OnAnswerClicked(int selectedAnswer)
        {
            if (_isAnswered) return;

            _isAnswered = true;
            Question currentQuestion = _quizData.questions[_currentQuestionIndex];
            bool isCorrect = selectedAnswer == currentQuestion.correctAnswerIndex; // Теперь поле

            HighlightAnswer(selectedAnswer, isCorrect, currentQuestion.correctAnswerIndex);
            DisableAnswerButtons();
            DisplayGuideText(isCorrect, currentQuestion);
        }

        private void HighlightAnswer(int selectedAnswer, bool isCorrect, int correctAnswerIndex)
        {
            Button[] answerButtons = _quizView.GetAnswerButtons();

            Outline selectedOutline = answerButtons[selectedAnswer].GetComponent<Outline>();
            if (selectedOutline != null)
            {
                selectedOutline.effectColor = isCorrect ? _greenOutline : _redOutline;
                selectedOutline.effectDistance = new Vector2(5, 5);
            }

            if (!isCorrect)
            {
                Outline correctOutline = answerButtons[correctAnswerIndex].GetComponent<Outline>();
                if (correctOutline != null)
                {
                    correctOutline.effectColor = _greenOutline;
                    correctOutline.effectDistance = new Vector2(5, 5);
                }
            }
        }

        private void DisableAnswerButtons()
        {
            foreach (var button in _quizView.GetAnswerButtons())
            {
                button.interactable = false;
            }
        }

        private void DisplayGuideText(bool isCorrect, Question question)
        {
            string guideMessage = isCorrect ? question.guideTextCorrect : question.guideTextIncorrect; // Теперь поля
            _textTyper.TypeText(guideMessage, 3f, () => _isGuideTextFinished = true);
        }

        private void MoveToNextQuestion()
        {
            _currentQuestionIndex++;
            LoadQuestion(_currentQuestionIndex);
        }

        private void EndQuiz()
        {
            Debug.Log("Квиз завершён!");
        }
    }
}