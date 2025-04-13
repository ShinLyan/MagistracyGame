using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class TextTyper : MonoBehaviour
{
	private TextMeshProUGUI textComponent;

	void Awake()
	{
		textComponent = GetComponent<TextMeshProUGUI>();
	}

	public void TypeText(string text, float charactersPerSecond, Action onComplete)
	{
		StopAllCoroutines();
		StartCoroutine(TypeTextCoroutine(text, charactersPerSecond, onComplete));
	}

	private IEnumerator TypeTextCoroutine(string text, float charactersPerSecond, Action onComplete)
	{
		textComponent.text = "";
		float delay = 1f / charactersPerSecond;

		foreach (char c in text)
		{
			textComponent.text += c;
			yield return new WaitForSeconds(delay);
		}

		onComplete?.Invoke();
	}
}