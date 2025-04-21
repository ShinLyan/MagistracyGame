using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MagoLegoMenu : MonoBehaviour
{
    [SerializeField] private List<Panel> _options;
    [SerializeField] private Button _webButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private string _webUrl = "https://www.hse.ru/ma/gamedev/";
    [SerializeField] private Sprite _activeButtonSprite;

    [FormerlySerializedAs("_dialogStage")] [SerializeField]
    private DialogueStage _dialogueStage;
    private int selectedIndex;

    void Start()
    {
        _nextButton.interactable = false;
        _webButton.onClick.AddListener(OnWebClicked);
        for (int i = 0; i < _options.Count; i++)
        {
            int idx = i;
            _options[i].GetComponent<Button>().onClick.AddListener(() => OnOptionClicked(idx));
        }
        
        _nextButton.onClick.AddListener(OnNextClicked);
    }

    void OnOptionClicked(int idx)
    {
        for (int i = 0; i < _options.Count; i++)
        {
            bool isActive = (i == idx);
            _options[i].SetActive(isActive);
        }
        selectedIndex = idx;

        _nextButton.interactable = true;
        _nextButton.GetComponent<Image>().sprite = _activeButtonSprite;
    }

    void OnWebClicked()
    {
        Application.OpenURL(_webUrl);
    }

    void OnNextClicked()
    {
        PlayerPrefs.SetString("SelectedMagoLego", _options[selectedIndex].transform.Find("Course").GetComponent<Text>().text);
        PlayerPrefs.Save();
        
        _dialogueStage.StartDialogue();
    }
}
