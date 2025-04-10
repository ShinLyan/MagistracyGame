using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private readonly Color _originalColor = Color.white;
    private Image _image;

    public RectTransform RectTransform { get; private set; }
    public char Letter { get; private set; }

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _image = GetComponent<Image>();
        RectTransform = GetComponent<RectTransform>();
    }

    public void SetLetter(char letter)
    {
        Letter = letter;
        _text.text = letter.ToString().ToUpper();
    }

    public void SetSelected(bool selected, Color selectionColor)
    {
        _image.color = selectionColor;
        _text.color = selected ? Color.white : Color.black;
    }

    public Vector2 GetPosition() => RectTransform.position;
}