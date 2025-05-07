using System.Collections;
using TMPro;
using UnityEngine;

public class TextTyper : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _text;
	private Coroutine _typingCoroutine;
	private string _fullText;

	public void TypeText(string text, float charactersPerSecond)
	{
		StopTyping();
		_fullText = text;
		_typingCoroutine = StartCoroutine(TypeTextCoroutine(text, charactersPerSecond));
	}

	public void CompleteTextImmediately()
	{
		StopTyping();
		_text.text = _fullText;
	}

	private void StopTyping()
	{
		if (_typingCoroutine != null)
		{
			StopCoroutine(_typingCoroutine);
			_typingCoroutine = null;
		}
	}

	private IEnumerator TypeTextCoroutine(string text, float charactersPerSecond)
	{
		_text.text = "";
		float delay = 1f / charactersPerSecond;

		foreach (char c in text)
		{
			_text.text += c;
			yield return new WaitForSeconds(delay);
		}

		_typingCoroutine = null;
	}
}