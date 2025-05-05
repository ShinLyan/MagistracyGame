using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MagistracyGame.Scripts.StudentCard.StudentCard

{
    public class StudentCard : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private Button _button;
        [SerializeField] private Image _stamp;
        [SerializeField] private RectTransform _studentCard;
        [SerializeField] private TMP_Text _dateText;
        [SerializeField] private TMP_Text _wordCountText;

        [FormerlySerializedAs("_dialogStage")] [SerializeField]
        private DialogueStage _dialogueStage;

        private const float StampFadeDuration = 1f;
        private const float PauseDuration = 2f;
        private const float SlideOutDuration = 0.5f;

        private void Start()
        {
            _nameInputField.onEndEdit.AddListener(OnNameEntered);
            _nameInputField.onValueChanged.AddListener(OnNameChanged);
        }

        private void OnNameEntered(string input)
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
                if (!string.IsNullOrWhiteSpace(input))
                {
                    _nameInputField.interactable = false;
                    _nameInputField.DeactivateInputField();
                    OnContinueClicked();
                }
        }

        private void OnNameChanged(string input)
        {
            _wordCountText.text = $"{input.Length}/500";
        }

        private void OnContinueClicked()
        {
            PlayerPrefs.SetString("PlayerNickname", _nameInputField.text);
            PlayerPrefs.Save();
            StartCoroutine(StampAnimationCoroutine());
        }

        private IEnumerator StampAnimationCoroutine()
        {
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
            _dialogueStage.StartDialogue();
        }
    }
}