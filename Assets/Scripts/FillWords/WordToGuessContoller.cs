using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MagistracyGame.FillWords
{
	public class WordToGuessController : MonoBehaviour
	{
		[SerializeField] private Image _backgroundImage;
		[SerializeField] private TextMeshProUGUI _wordText;
		[SerializeField] private Sprite _defaultSprite;
		[SerializeField] private Sprite _foundSprite;

		private bool _isFound;

		private void Awake()
		{
			if (_backgroundImage == null) _backgroundImage = GetComponent<Image>();
			if (_wordText == null) _wordText = GetComponentInChildren<TextMeshProUGUI>();
			SetSprite(_defaultSprite);
		}

		private void SetSprite(Sprite sprite)
		{
			if (_backgroundImage != null && sprite != null)
			{
				_backgroundImage.sprite = sprite;
			}
		}

		public void SetWord(string word)
		{
			_wordText.text = word.ToUpper();
		}

		public void MarkAsFound()
		{
			if (!_isFound)
			{
				_isFound = true;
				SetSprite(_foundSprite);
				_wordText.color = Color.gray;
			}
		}

		
	}
}