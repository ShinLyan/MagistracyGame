namespace QuizManager
{
	using UnityEngine;
	using TMPro;
	using UnityEngine.UI;
	using QuizData;

	public class QuizView : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _questionText;
		[SerializeField] private TextMeshProUGUI _guideText;
		[SerializeField] private Button[] _answerButtons;

		public void DisplayQuestion(Question question)
		{
			_questionText.text = question.questionText;
			_guideText.text = question.initialGuideText;

			for (int i = 0; i < _answerButtons.Length; i++)
			{
				_answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = question.answers[i];
				_answerButtons[i].interactable = true;

				Outline outline = _answerButtons[i].GetComponent<Outline>();
				if (outline != null)
				{
					outline.effectColor = new Color(0, 0, 0, 0);
					outline.effectDistance = new Vector2(5, 5);
				}
			}
		}

		public Button[] GetAnswerButtons()
		{
			return _answerButtons;
		}
	}
}