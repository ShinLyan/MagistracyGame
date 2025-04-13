using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private QuizData quizData;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private TextMeshProUGUI guideText;
    [SerializeField] private Color greenOutline = new Color(0, 1, 0, 1);
    [SerializeField] private Color redOutline = new Color(1, 0, 0, 1);

    private int currentQuestionIndex = 0;
    private bool isAnswered = false;
    private bool isGuideTextFinished = false;
    private TextTyper textTyper;

    void Start()
    {
        textTyper = guideText.GetComponent<TextTyper>();
        LoadQuestion(currentQuestionIndex);
        EventSystem.current.SetSelectedGameObject(null);
    }

    void Update()
    {
        if (isAnswered && isGuideTextFinished && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            MoveToNextQuestion();
        }
    }

    void LoadQuestion(int index)
    {
        if (index >= quizData.questions.Length) return;

        isAnswered = false;
        isGuideTextFinished = false;
        questionText.text = quizData.questions[index].questionText;
        string initialText = quizData.questions[index].initialGuideText;
        guideText.text = initialText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int answerIndex = i;
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = quizData.questions[index].answers[i];
            answerButtons[i].interactable = true;
            Outline outline = answerButtons[i].GetComponent<Outline>();
            if (outline != null)
            {
                outline.effectColor = new Color(0, 0, 0, 0);
                outline.effectDistance = new Vector2(5, 5);
            }
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerClicked(answerIndex));
        }
    }

    void OnAnswerClicked(int selectedAnswer)
    {
        if (isAnswered) return;

        isAnswered = true;
        QuizData.Question currentQuestion = quizData.questions[currentQuestionIndex];
        bool isCorrect = selectedAnswer == currentQuestion.correctAnswerIndex;

        Outline selectedOutline = answerButtons[selectedAnswer].GetComponent<Outline>();
        if (selectedOutline != null)
        {
            selectedOutline.effectColor = isCorrect ? greenOutline : redOutline;
            selectedOutline.effectDistance = new Vector2(5, 5);
        }

        if (!isCorrect)
        {
            Outline correctOutline = answerButtons[currentQuestion.correctAnswerIndex].GetComponent<Outline>();
            if (correctOutline != null)
            {
                correctOutline.effectColor = greenOutline;
                correctOutline.effectDistance = new Vector2(5, 5);
            }
        }

        foreach (var button in answerButtons)
        {
            button.interactable = false;
        }

        string guideMessage = isCorrect ? currentQuestion.guideTextCorrect : currentQuestion.guideTextIncorrect;
        textTyper.TypeText(guideMessage, 3f, () => isGuideTextFinished = true);
    }

    void MoveToNextQuestion()
    {
        currentQuestionIndex++;
        if (currentQuestionIndex < quizData.questions.Length)
        {
            LoadQuestion(currentQuestionIndex);
        }
        else
        {
            Debug.Log("Квиз завершён!");
        }
    }
}