using System.Collections.Generic;
using MagistracyGame.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagoLegoMenu : MonoBehaviour
{
    [SerializeField] private List<Panel> _options;
    [SerializeField] private Button _magoLegoButton;
    [SerializeField] private Button _continueButton;

    private const string MagoLegoUrl = "https://electives.hse.ru/mg_oi/";

    private int selectedIndex;

    private void Awake()
    {
        _magoLegoButton.onClick.AddListener(() => Application.OpenURL(MagoLegoUrl));

        _continueButton.interactable = false;
    }

    private void Start()
    {
        for (int i = 0; i < _options.Count; i++)
        {
            int index = i;
            _options[i].GetComponent<Button>().onClick.AddListener(() => OnOptionClicked(index));
        }
    }

    private void OnOptionClicked(int index)
    {
        for (int i = 0; i < _options.Count; i++)
            if (i == index)
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

        selectedIndex = index;
        SaveChoice();
        _continueButton.interactable = true;
        _continueButton.GetComponentInChildren<TMP_Text>().color = Color.white;
        _continueButton.GetComponent<UIButton>().SetInteractable(true);
    }

    private void SaveChoice()
    {
        PlayerPrefs.SetString("SelectedMagoLego", _options[selectedIndex].transform.Find("Course").GetComponent<TMP_Text>().text);
        PlayerPrefs.Save();        
    }
}