using UnityEngine;

[CreateAssetMenu(fileName = "NewQuizData", menuName = "Quiz/QuizData")]
public class QuizData : ScriptableObject
{
	[System.Serializable]
	public class Question
	{
		[TextArea] public string questionText;
		public string[] answers = new string[4];
		public int correctAnswerIndex;
		[TextArea] public string initialGuideText;
		[TextArea] public string guideTextCorrect;
		[TextArea] public string guideTextIncorrect;
	}

	public Question[] questions;
}