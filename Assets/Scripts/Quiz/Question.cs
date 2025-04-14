namespace QuizData
{
	using UnityEngine;

	[System.Serializable]
	public class Question
	{
		[TextArea] public string questionText;
		public string[] answers;
		public int correctAnswerIndex;
		[TextArea] public string initialGuideText;
		[TextArea] public string guideTextCorrect;
		[TextArea] public string guideTextIncorrect;
	}
}