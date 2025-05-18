using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MagistracyGame.FillWords
{
    [RequireComponent(typeof(Image))]
    public class Word : MonoBehaviour
    {
        [SerializeField] private Sprite _defaultSprite;
        [SerializeField] private Sprite _foundSprite;

        private Image _backgroundImage;
        private TextMeshProUGUI _wordText;
        private bool _isFound;

        private void Awake()
        {
            _backgroundImage = GetComponent<Image>();
            _wordText = GetComponentInChildren<TextMeshProUGUI>();
            SetSprite(_defaultSprite);
        }

        private void SetSprite(Sprite sprite) => _backgroundImage.sprite = sprite;

        public void SetWord(string word) => _wordText.text = word.ToUpper();

        public void MarkAsFound()
        {
            if (_isFound) return;

            _isFound = true;
            SetSprite(_foundSprite);
            _wordText.color = Color.gray;
        }
    }
}