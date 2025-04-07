using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
	private TextMeshProUGUI text;
	private RectTransform rectTransform;
	private Color originalColor = Color.white;
	public bool isSelected { get; private set; } = false;
	public char letter { get; private set; }

	private void Awake()
	{
		text = GetComponentInChildren<TextMeshProUGUI>();
		rectTransform = GetComponent<RectTransform>();
		if (!string.IsNullOrEmpty(text.text) && text.text.Length == 1)
		{
			letter = text.text[0];
		}
	}

	public void SetLetter(char letter)
	{
		this.letter = letter;
		text.text = letter.ToString().ToUpper();
	}

	public void SetSelected(bool selected, Color selectionColor)
	{
		isSelected = selected;
		GetComponent<UnityEngine.UI.Image>().color = selected ? selectionColor : originalColor;
		text.color = selected ? Color.white : Color.black;
	}

	public Vector2 GetPosition()
	{
		return rectTransform.position;
	}
}