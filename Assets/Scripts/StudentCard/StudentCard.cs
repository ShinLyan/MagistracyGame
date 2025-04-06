using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MagistracyGame.Scripts.StudentCard.StudentCard

{
    public class StudentCard : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private Button _button;
        [SerializeField] private Image _stamp;
        [SerializeField] private RectTransform _studentCard;
        [SerializeField] private Sprite _yellowButtonSprite;

        private const float StampFadeDuration = 1f;
        private const float PauseDuration = 2f;
        private const float SlideOutDuration = 0.5f;

        private void Start()
        {
            _button.onClick.AddListener(OnContinueClicked);
            _nameInputField.onEndEdit.AddListener(OnNameEntered);
        }

        private void OnNameEntered(string input)
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            {
                if (!string.IsNullOrWhiteSpace(input))
                {
                    _button.interactable = true;
                    _nameInputField.interactable = false;
                    _button.image.sprite = _yellowButtonSprite;
                    _nameInputField.DeactivateInputField();
                }
            }
        }

        private void OnContinueClicked()
        {
            PlayerPrefs.SetString("PlayerNickname", _nameInputField.text);
            PlayerPrefs.Save();
            _button.interactable = false;
            StartCoroutine(StampAnimationCoroutine());
        }

        private IEnumerator StampAnimationCoroutine()
        {
            float timer = 0;
            while (timer < StampFadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(0, 1, timer / StampFadeDuration);
                _stamp.color = new Color(1, 1, 1, alpha);
                yield return null;
            }

            yield return new WaitForSeconds(PauseDuration);

            Vector2 startPosition = _studentCard.anchoredPosition;
            Vector2 endPosition = startPosition + new Vector2(0, Screen.height);

            timer = 0;
            while (timer < SlideOutDuration)
            {
                timer += Time.deltaTime;
                _studentCard.anchoredPosition = Vector2.Lerp(startPosition, endPosition, timer / SlideOutDuration);
                yield return null;
            }
        }
    }
}