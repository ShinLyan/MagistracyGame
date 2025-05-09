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
            if (i == idx)
            {
                _options[i].transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(0, 0, 0, 255);
                _options[i].transform.GetChild(1).GetComponent<TMP_Text>().color = new Color32(76, 76, 76, 255);
                _options[i].transform.GetChild(2).GetComponent<TMP_Text>().color = new Color32(51, 51, 51, 255);
                _options[i].SetActive(true);
            } 
            else
            {
                _options[i].transform.GetChild(0).GetComponent<TMP_Text>().color = new Color32(128, 128, 128, 255);
                _options[i].transform.GetChild(1).GetComponent<TMP_Text>().color = new Color32(153, 153, 153, 255);
                _options[i].transform.GetChild(2).GetComponent<TMP_Text>().color = new Color32(128, 128, 128, 255);
                _options[i].SetActive(false);
            }
            
        }
        selectedIndex = idx;

        _nextButton.interactable = true;
        _nextButton.GetComponentInChildren<TMP_Text>().color = Color.white;
        _nextButton.GetComponent<Image>().sprite = _activeButtonSprite;
    }

    void OnWebClicked()
    {
        Application.OpenURL(_webUrl);
    }

    void OnNextClicked()
    {
        PlayerPrefs.SetString("SelectedMagoLego", _options[selectedIndex].transform.Find("Course").GetComponent<TMP_Text>().text);
        PlayerPrefs.Save();
        
        _dialogueStage.StartDialogue();
    }
}
