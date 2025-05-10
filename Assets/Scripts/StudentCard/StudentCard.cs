using System;
using System.Collections;
using MagistracyGame.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MagistracyGame.StudentCard
{
    public class StudentCard : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private Image _stamp;
        [SerializeField] private RectTransform _studentCard;
        [SerializeField] private TMP_Text _dateText;
        [SerializeField] private TMP_Text _wordCountText;
        [SerializeField] private UIButton _continueButton;

        private void Awake()
        {
            _nameInputField.onValueChanged.AddListener(OnNameChanged);
            _continueButton.Button.onClick.AddListener(OnClickContinue);
        }

        private void Start()
        {
            _continueButton.SetInteractable(false);
        }

        private void OnNameChanged(string input)
        {
            _wordCountText.text = $"{input.Length}/500";

            _continueButton.SetInteractable(!string.IsNullOrWhiteSpace(input));
        }

        private void OnClickContinue()
        {
            _continueButton.SetInteractable(false);
            _nameInputField.interactable = false;
            _nameInputField.DeactivateInputField();

            PlayerPrefs.SetString("PlayerNickname", _nameInputField.text);
            PlayerPrefs.Save();
            StartCoroutine(StampAnimationCoroutine());
        }

        private IEnumerator StampAnimationCoroutine()
        {
            const float StampFadeDuration = 1f;
            const float PauseDuration = 2f;
            const float SlideOutDuration = 0.5f;

            float timer = 0;
            _dateText.text = DateTime.Now.ToString("dd.MM.yyyy");
            while (timer < StampFadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(0, 1, timer / StampFadeDuration);
                _stamp.color = new Color(1, 1, 1, alpha);
                yield return null;
            }

            yield return new WaitForSeconds(PauseDuration);

            var startPosition = _studentCard.anchoredPosition;
            var endPosition = startPosition + new Vector2(0, Screen.height);

            timer = 0;
            while (timer < SlideOutDuration)
            {
                timer += Time.deltaTime;
                _studentCard.anchoredPosition = Vector2.Lerp(startPosition, endPosition, timer / SlideOutDuration);

                yield return null;
            }

            yield return new WaitForSeconds(SlideOutDuration);
            FindFirstObjectByType<DialogueStage>().StartDialogue();
        }
    }
}