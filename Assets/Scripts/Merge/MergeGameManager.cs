using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using MagistracyGame.Merge;
using UnityEngine.Serialization;


namespace MagistracyGame.Merge
{
    public class MergeGameManager : MonoBehaviour
    {
        [Header("UI Elements")] [SerializeField]
        private Image _slotFirst;

        [SerializeField] private Image _slotSecond;
        [SerializeField] private TextMeshProUGUI _slotFirstText;
        [SerializeField] private TextMeshProUGUI _slotSecondText;
        [SerializeField] private Button _mergeButton;
        [SerializeField] private Image _resultSlot;
        [SerializeField] private TextMeshProUGUI _resultText;
        [SerializeField] private GameObject _availableElementsPanel;
        [SerializeField] private GameObject _victoryPanel;
        [SerializeField] private Image _victoryIcon;
        [SerializeField] private TextMeshProUGUI _victoryTitle;
        [SerializeField] private TextMeshProUGUI _victoryDescription;

        [Header("Icons")] [SerializeField] private Sprite _mathIcon;
        [SerializeField] private Sprite _physicsIcon;
        [SerializeField] private Sprite _programmingIcon;
        [SerializeField] private Sprite _mathModelIcon;
        [SerializeField] private Sprite _databaseIcon;
        [SerializeField] private Sprite _neuralNetIcon;
        [SerializeField] private Sprite _physicsModuleIcon;
        [SerializeField] private Sprite _neuralModuleIcon;
        [SerializeField] private Sprite _codeIcon;
        [SerializeField] private Sprite _failIcon;

        [SerializeField] private MergeGameData _gameData;
        [SerializeField] private GameObject _elementButtonPrefab;
        [SerializeField] private CanvasGroup _mainUIGroup;
        private HashSet<string> _collectedFinalElements = new HashSet<string>();
        private HashSet<string> _allFinalElements = new HashSet<string> { "Модуль физики", "Нейронный модуль" };
        private List<string> _availableElements = new List<string> { "Математика", "Физика", "Программирование" };
        private Dictionary<(string, string), string> _mergeRules = new Dictionary<(string, string), string>();
        private Dictionary<string, Sprite> _elementIcons = new Dictionary<string, Sprite>();
        private bool _hasShownFinalText;
        private string _slotFirstElement = null;
        private string _slotSecondElement = null;
        private bool _hasWon;

        private string _congratulations =
            "Поздравляем! Вы создали финальный элемент.\nТеперь вы можете перейти к диалогу с руководителем.";

        private void Awake()
        {
            InitializeData();
            _mergeButton.onClick.AddListener(OnMergeButtonClick);
        }

        private void Start()
        {
            _victoryPanel.SetActive(false);
            UpdateAvailableElements();
        }

        void Update()
        {
            if (!_hasShownFinalText && _victoryPanel.activeSelf && (Input.GetKeyDown(KeyCode.Return) ||
                    Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
            {
                StopAllCoroutines();
                _victoryDescription.text = _congratulations;
                _hasShownFinalText = true;
                ShowFinalDialog();
            }
        }

        private void InitializeData()
        {
            _mergeRules.Clear();
            foreach (var rule in _gameData.MergeRules)
            {
                _mergeRules.Add((rule.Element1, rule.Element2), rule.Result);
            }

            _elementIcons.Clear();
            foreach (var elementIcon in _gameData.ElementIcons)
            {
                _elementIcons[elementIcon.ElementName] = elementIcon.Icon;
            }

            _availableElements = new List<string>(_gameData.AvailableElements);
            _allFinalElements = new HashSet<string>(_gameData.FinalElements);
        }


        public void OnElementClick(string element)
        {
            if (_slotFirstElement == null)
            {
                _slotFirstElement = element;
                _slotFirst.sprite = _elementIcons[element];
                _slotFirstText.text = element;
            }
            else if (_slotSecondElement == null)
            {
                _slotSecondElement = element;
                _slotSecond.sprite = _elementIcons[element];
                _slotSecondText.text = element;
            }

            if (_slotFirstElement != null && _slotSecondElement != null)
            {
                _mergeButton.GetComponent<Image>().color = Color.yellow;
            }
        }

        void OnMergeButtonClick()
        {
            if (_slotFirstElement == null || _slotSecondElement == null) return;

            string result = TryMerge(_slotFirstElement, _slotSecondElement);
            if (result == null)
            {
                StartCoroutine(ShowFailResult());
            }
            else
            {
                StartCoroutine(ShowSuccessResult(result));
            }
        }

        string TryMerge(string element1, string element2)
        {
            var key1 = (element1, element2);
            var key2 = (element2, element1);
            if (_mergeRules.ContainsKey(key1)) return _mergeRules[key1];
            if (_mergeRules.ContainsKey(key2)) return _mergeRules[key2];
            return null;
        }

        IEnumerator ShowFailResult()
        {
            _resultSlot.sprite = _failIcon;
            yield return new WaitForSeconds(2f);
            ResetSlots();
        }

        IEnumerator ShowSuccessResult(string resultElement)
        {
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


        void ResetSlots()
        {
            _slotFirstElement = null;
            _slotSecondElement = null;
            _slotFirst.sprite = default;
            _slotSecond.sprite = default;
            _slotFirstText.text = "";
            _slotSecondText.text = "";
            _resultSlot.sprite = default;
            _resultText.text = "";
            _mergeButton.GetComponent<Image>().color = Color.gray;
        }

        void UpdateAvailableElements()
        {
            foreach (Transform child in _availableElementsPanel.transform)
            {
                Destroy(child.gameObject);
            }

            float xOffset = 0f;
            float paddingX = 20f;
            float paddingY = 20f;

            foreach (string element in _availableElements)
            {
                GameObject elementButton = Instantiate(_elementButtonPrefab, _availableElementsPanel.transform);

                Image img = elementButton.GetComponent<Image>();
                if (img != null)
                    img.sprite = _elementIcons[element];

                Button btn = elementButton.GetComponent<Button>();
                if (btn != null)
                    btn.onClick.AddListener(() => OnElementClick(element));

                RectTransform rect = elementButton.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0f, 1f);
                rect.anchorMax = new Vector2(0f, 1f);
                rect.pivot = new Vector2(0f, 1f);
                rect.anchoredPosition = new Vector2(paddingX + xOffset, -paddingY);
                rect.sizeDelta = new Vector2(130, 130);

                xOffset += 200f;
            }
        }


        IEnumerator ShowVictoryScreen(string finalElement)
        {
            const float fadeDuration = 1f;
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                _mainUIGroup.alpha = alpha;
                yield return null;
            }

            _mainUIGroup.interactable = false;
            _mainUIGroup.blocksRaycasts = false;

            _victoryPanel.SetActive(true);

            string fullText = _congratulations;
            const float charPerSecond = 3f;
            foreach (char symbol in fullText)
            {
                _victoryDescription.text += symbol;
                yield return new WaitForSeconds(1f / charPerSecond);
            }
        }

        void ShowFinalDialog()
        {
            Debug.Log("Переход к диалогу с руководителем...");
        }

    }

}