using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MergeGameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image slot1;
    public Image slot2;
    public TextMeshProUGUI slot1Text;
    public TextMeshProUGUI slot2Text;
    public Button mergeButton;
    public Image resultSlot;
    public TextMeshProUGUI resultText;
    public GameObject availableElementsPanel;
    public GameObject victoryPanel;
    public Image victoryIcon;
    public TextMeshProUGUI victoryTitle;
    public TextMeshProUGUI victoryDescription;

    [Header("Icons")]
    public Sprite mathIcon;
    public Sprite physicsIcon;
    public Sprite programmingIcon;
    public Sprite mathModelIcon;
    public Sprite databaseIcon;
    public Sprite neuralNetIcon;
    public Sprite physicsModuleIcon;
    public Sprite neuralModuleIcon;
    public Sprite codeIcon;
    public Sprite failIcon;

    [SerializeField] private CanvasGroup mainUIGroup;
    private HashSet<string> collectedFinalElements = new HashSet<string>();
    private HashSet<string> allFinalElements = new HashSet<string> { "Модуль физики", "Нейронный модуль" };
    private List<string> availableElements = new List<string> { "Математика", "Физика", "Программирование" };
    private Dictionary<(string, string), string> mergeRules = new Dictionary<(string, string), string>();
    private Dictionary<string, Sprite> elementIcons = new Dictionary<string, Sprite>();
    private bool hasShownFinalText;
    private string slot1Element = null;
    private string slot2Element = null;
    private bool hasWon;
    private string congratulations = "Поздравляем! Вы создали финальный элемент.\nТеперь вы можете перейти к диалогу с руководителем.";

    void Start()
    {
        victoryPanel.SetActive(false);
        
        mergeRules.Add(("Математика", "Физика"), "Математическая модель");
        mergeRules.Add(("Программирование", "Математика"), "Нейросеть");
        mergeRules.Add(("Программирование", "Программирование"), "Код");
        mergeRules.Add(("Математическая модель", "Код"), "Модуль физики");
        mergeRules.Add(("Нейросеть", "Код"), "Нейронный модуль");

        elementIcons["Математика"] = mathIcon;
        elementIcons["Физика"] = physicsIcon;
        elementIcons["Программирование"] = programmingIcon;
        elementIcons["Математическая модель"] = mathModelIcon;
        elementIcons["Нейросеть"] = neuralNetIcon;
        elementIcons["Код"] = codeIcon;
        elementIcons["Модуль физики"] = physicsModuleIcon;
        elementIcons["Нейронный модуль"] = neuralModuleIcon;

        UpdateAvailableElements();
        mergeButton.onClick.AddListener(OnMergeButtonClick);
    }
    
    void Update()
    {
        if (!hasShownFinalText && victoryPanel.activeSelf && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            StopAllCoroutines();
            victoryDescription.text = congratulations;
            hasShownFinalText = true;
            ShowFinalDialog();
        }
    }
    

    public void OnElementClick(string element)
    {
        if (slot1Element == null)
        {
            slot1Element = element;
            slot1.sprite = elementIcons[element];
            slot1Text.text = element;
        }
        else if (slot2Element == null)
        {
            slot2Element = element;
            slot2.sprite = elementIcons[element];
            slot2Text.text = element;
        }

        if (slot1Element != null && slot2Element != null)
        {
            mergeButton.GetComponent<Image>().color = Color.yellow;
        }
    }

    void OnMergeButtonClick()
    {
        if (slot1Element == null || slot2Element == null) return;

        string result = TryMerge(slot1Element, slot2Element);
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
        if (mergeRules.ContainsKey(key1)) return mergeRules[key1];
        if (mergeRules.ContainsKey(key2)) return mergeRules[key2];
        return null;
    }

    IEnumerator ShowFailResult()
    {
        resultSlot.sprite = failIcon;
        resultText.text = "";
        yield return new WaitForSeconds(2f);
        ResetSlots();
    }

    IEnumerator ShowSuccessResult(string resultElement)
    {
        resultSlot.sprite = elementIcons[resultElement];
        resultText.text = resultElement;
        yield return new WaitForSeconds(2f);

        if (allFinalElements.Contains(resultElement))
        {
            collectedFinalElements.Add(resultElement);

            if (collectedFinalElements.SetEquals(allFinalElements))
            {
                StartCoroutine(ShowVictoryScreen(resultElement));
                yield break;
            }
        }

        if (!availableElements.Contains(resultElement))
        {
            availableElements.Add(resultElement);
            UpdateAvailableElements();
        }
        ResetSlots();
    }


    void ResetSlots()
    {
        slot1Element = null;
        slot2Element = null;
        slot1.sprite = default;
        slot2.sprite = default;
        slot1Text.text = "";
        slot2Text.text = "";
        resultSlot.sprite = default;
        resultText.text = "";
        mergeButton.GetComponent<Image>().color = Color.gray;
    }

    void UpdateAvailableElements()
    {
        foreach (Transform child in availableElementsPanel.transform)
        {
            Destroy(child.gameObject);
        }

        float xOffset = 0f;
        float paddingX = 20f;
        float paddingY = 20f;

        foreach (string element in availableElements)
        {
            GameObject elementButton = new GameObject(element);
            elementButton.transform.SetParent(availableElementsPanel.transform);

            Image img = elementButton.AddComponent<Image>();
            img.sprite = elementIcons[element];

            Button btn = elementButton.AddComponent<Button>();
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
        float fadeDuration = 1f;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            mainUIGroup.alpha = alpha;
            yield return null;
        }

        mainUIGroup.interactable = false;
        mainUIGroup.blocksRaycasts = false;

        victoryPanel.SetActive(true);

        string fullText = congratulations;
        victoryDescription.text = "";
        float charPerSecond = 3f;
        foreach (char c in fullText)
        {
            victoryDescription.text += c;
            yield return new WaitForSeconds(1f / charPerSecond);
        }
    }
    
    void ShowFinalDialog()
    {
        Debug.Log("Переход к диалогу с руководителем...");
    }
    
}
    