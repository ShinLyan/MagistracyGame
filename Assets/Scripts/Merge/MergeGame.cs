using System.Collections;
using System.Collections.Generic;
using MagistracyGame.Core;
using MagistracyGame.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MagistracyGame.Merge
{
    [RequireComponent(typeof(CanvasGroup))]
    public class MergeGame : MonoBehaviour, IGame
    {
        [Header("UI Elements")] [SerializeField]
        private Image _slotFirst;

        [SerializeField] private Image _slotSecond;
        [SerializeField] private TextMeshProUGUI _slotFirstText;
        [SerializeField] private TextMeshProUGUI _slotSecondText;
        [SerializeField] private UIButton _mergeButton;
        [SerializeField] private Image _resultSlot;
        [SerializeField] private TextMeshProUGUI _resultText;
        [SerializeField] private GameObject _elementsContainer;
        [SerializeField] private GameObject _elementButtonPrefab;
        [SerializeField] private MergeGameData _gameData;
        [SerializeField] private GameObject _startPanel;

        [Header("Win Panel")]
        [SerializeField] private CanvasGroup _winPanel;

        [SerializeField] private Image _winIcon;
        [SerializeField] private TextMeshProUGUI _winIconText;

        [Header("Icons")]
        [SerializeField] private Sprite _failIcon;

        [SerializeField] private Sprite _defaultIcon;

        private HashSet<string> _allFinalElements = new() { "Модуль физики", "Нейронный модуль" };
        private List<string> _availableElements = new() { "Математика", "Физика", "Программирование" };
        private readonly Dictionary<(string, string), string> _mergeRules = new();
        private readonly Dictionary<string, Sprite> _elementIcons = new();
        private string _firstElement;
        private string _secondElement;
        private bool _hasWon;

        private bool _canClick = true;

        private void Awake()
        {
            InitializeData();
            _mergeButton.Button.onClick.AddListener(OnMergeButtonClick);
        }

        private void Start()
        {
            _startPanel.SetActive(true);
            UpdateAvailableElements();
        }

        private void InitializeData()
        {
            _mergeRules.Clear();
            foreach (var rule in _gameData.MergeRules)
                _mergeRules.Add((rule.Element1, rule.Element2), rule.Result);

            _elementIcons.Clear();
            foreach (var elementIcon in _gameData.ElementIcons)
                _elementIcons[elementIcon.ElementName] = elementIcon.Icon;

            _availableElements = new List<string>(_gameData.AvailableElements);
            _allFinalElements = new HashSet<string>(_gameData.FinalElements);
        }

        private void OnElementClick(string element)
        {
            if (!_canClick) return;

            if (_firstElement == null)
            {
                _firstElement = element;
                _slotFirst.sprite = _elementIcons[element];
                _slotFirstText.text = element;
            }
            else if (_secondElement == null)
            {
                _secondElement = element;
                _slotSecond.sprite = _elementIcons[element];
                _slotSecondText.text = element;
            }

            if (_firstElement != null && _secondElement != null)
                _mergeButton.SetInteractable(true);
        }

        private void OnMergeButtonClick()
        {
            if (_firstElement == null || _secondElement == null) return;

            string result = TryMerge(_firstElement, _secondElement);
            StartCoroutine(result == null ? ShowFailResult() : ShowSuccessResult(result));
        }

        private string TryMerge(string element1, string element2)
        {
            var key1 = (element1, element2);
            var key2 = (element2, element1);
            if (_mergeRules.TryGetValue(key1, out string merge)) return merge;
            if (_mergeRules.TryGetValue(key2, out string tryMerge)) return tryMerge;
            return null;
        }

        private IEnumerator ShowFailResult()
        {
            _canClick = false;
            _resultSlot.sprite = _failIcon;
            yield return new WaitForSeconds(2f);
            ResetSlots();
        }

        private IEnumerator ShowSuccessResult(string resultElement)
        {
            _canClick = false;

            _resultSlot.sprite = _elementIcons[resultElement];
            _resultText.text = resultElement;
            yield return new WaitForSeconds(2f);

            if (_allFinalElements.Contains(resultElement))
            {
                StartCoroutine(ShowVictoryScreen(resultElement));
                yield break;
            }

            if (!_availableElements.Contains(resultElement))
            {
                _availableElements.Add(resultElement);
                UpdateAvailableElements();
            }

            ResetSlots();
        }


        private void ResetSlots()
        {
            _canClick = true;

            _firstElement = null;
            _secondElement = null;

            _slotFirst.sprite = _defaultIcon;
            _slotSecond.sprite = _defaultIcon;
            _resultSlot.sprite = _defaultIcon;
            _slotFirstText.text = "";
            _slotSecondText.text = "";
            _resultText.text = "";

            _mergeButton.SetInteractable(false);
        }

        private void UpdateAvailableElements()
        {
            foreach (Transform child in _elementsContainer.transform)
                Destroy(child.gameObject);

            foreach (string element in _availableElements)
            {
                var elementButton = Instantiate(_elementButtonPrefab, _elementsContainer.transform);

                var image = elementButton.GetComponent<Image>();
                if (image) image.sprite = _elementIcons[element];

                var text = elementButton.GetComponentInChildren<TextMeshProUGUI>();
                if (text) text.text = element;

                var button = elementButton.GetComponent<Button>();
                if (button) button.onClick.AddListener(() => OnElementClick(element));
            }
        }

        private IEnumerator ShowVictoryScreen(string finalElement)
        {
            const float FadeDuration = 1f;
            float elapsed = 0f;
            var mergeGameUI = GetComponent<CanvasGroup>();

            while (elapsed < FadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / FadeDuration);
                mergeGameUI.alpha = alpha;
                yield return null;
            }

            mergeGameUI.interactable = false;
            mergeGameUI.blocksRaycasts = false;

            _winPanel.alpha = 0f;
            _winIconText.text = finalElement;
            _winIcon.sprite = _elementIcons[finalElement];
            _winPanel.gameObject.SetActive(true);

            elapsed = 0f;
            while (elapsed < FadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, elapsed / FadeDuration);
                _winPanel.alpha = alpha;
                yield return null;
            }

            PlayerPrefs.SetString("CompletedPractice", finalElement);
            yield return new WaitForSeconds(1f);
            FinishGame();
        }

        #region IGame

        public bool IsGameFinished { get; private set; }

        [field: SerializeField] public UnityEvent OnGameFinished { get; private set; }

        public void FinishGame()
        {
            IsGameFinished = true;
            OnGameFinished?.Invoke();
        }

        #endregion
    }
}