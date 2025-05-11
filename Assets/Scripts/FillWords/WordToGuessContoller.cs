using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MagistracyGame.FillWords
{
    [RequireComponent(typeof(Image))]
    public class Word : MonoBehaviour
    {
        [SerializeField] private Sprite _foundSprite;

        private Image _backgroundImage;
        private TextMeshProUGUI _wordText;
        private bool _isFound;

        private void Awake()
        {
            _backgroundImage = GetComponent<Image>();
            _wordText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void SetWord(string word)
        {
            _wordText.text = word.ToUpper();
        }

        public void MarkAsFound()
        {
            if (_isFound) return;

            _isFound = true;
            _backgroundImage.sprite = _foundSprite;
            _wordText.color = Color.gray;
        }
    }
}