namespace QuizData
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "NewQuizData", menuName = "Quiz/QuizData")]
	public class QuizData : ScriptableObject
	{
		public Question[] questions;
	}
}