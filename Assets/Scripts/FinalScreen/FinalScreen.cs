using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MagistracyGame.FinalScreen
{
    public class FinalScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _practiceText;
        [SerializeField] private TMP_Text _magoLegoText;
        [SerializeField] private TMP_Text _dateText;
        [SerializeField] private Button _webButton;

        private const string ProgramUrl = "https://www.hse.ru/ma/gamedev/";

        private void Awake()
        {
            _webButton.onClick.AddListener(() => Application.OpenURL(ProgramUrl));
            LoadPlayerData();
            _dateText.text = DateTime.Now.ToString("dd.MM.yyyy");
        }

        private void LoadPlayerData()
        {
            _nameText.text = PlayerPrefs.GetString("PlayerNickname");
            _practiceText.text = PlayerPrefs.GetString("CompletedPractice");
            _magoLegoText.text = PlayerPrefs.GetString("SelectedMagoLego");
        }
    }
}