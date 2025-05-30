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
        [SerializeField] private GameObject _errorText;
        [SerializeField] private UIButton _continueButton;
        [SerializeField] private NicknameFilter _nicknameFilter;
        [SerializeField] private Sprite _grayInput;
        [SerializeField] private Sprite _redInput;
        [SerializeField] private RectTransform _taskPanel;

        private void Awake()
        {
            _nameInputField.onValueChanged.AddListener(OnNameChanged);
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

        public void OnClickContinue()
        {
            string nickname = _nameInputField.text;

            if (!_nicknameFilter.IsNicknameClean(nickname))
            {
                _nameInputField.GetComponent<Image>().sprite = _redInput;
                _nameInputField.text = "";
                _errorText.SetActive(true);
                if (_nameInputField.placeholder is TMP_Text placeholder) placeholder.text = "Неприемлемое имя";
                return;
            }

            _nameInputField.GetComponent<Image>().sprite = _grayInput;
            _errorText.SetActive(false);
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
            const float PauseDuration = 1f;
            const float SlideOutDuration = 0.5f;

            float timer = 0;
            _dateText.text = DateTime.Now.ToString("dd.MM.yyyy");
            while (timer < StampFadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(0, 1, timer / StampFadeDuration);
                _stamp.color = new Color(1, 1, 1, alpha);
                _dateText.color = new Color(0, 0, 0, alpha);
                yield return null;
            }

            yield return new WaitForSeconds(PauseDuration);

            var startPosition = _studentCard.anchoredPosition;
            var endPosition = startPosition + new Vector2(0, Screen.height);

            var startPanelPos = _taskPanel.anchoredPosition;
            var endPanelPosition = startPanelPos - new Vector2(0, Screen.height);

            timer = 0;
            while (timer < SlideOutDuration)
            {
                timer += Time.deltaTime;
                _studentCard.anchoredPosition = Vector2.Lerp(startPosition, endPosition, timer / SlideOutDuration);
                _taskPanel.anchoredPosition = Vector2.Lerp(startPanelPos, endPanelPosition, timer / SlideOutDuration);
                yield return null;
            }

            yield return new WaitForSeconds(SlideOutDuration);
            FindFirstObjectByType<DialogueStage>().StartDialogue();
        }
    }
}