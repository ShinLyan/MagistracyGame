using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MagistracyGame.FillWords
{
    public class Tile : MonoBehaviour
    {
        private TextMeshProUGUI _text;
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
}