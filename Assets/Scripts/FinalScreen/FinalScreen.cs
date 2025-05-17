using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MagistracyGame.FinalScreen
{
    public class FinalScreen : MonoBehaviour
    {
        [SerializeField] private Button _diplomaButton;
        [SerializeField] private Button _webButton;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _practiceText;
        [SerializeField] private TMP_Text _magoLegoText;
        [SerializeField] private TMP_Text _dateText;

        private const string ProgramUrl = "https://www.hse.ru/ma/gamedev/";
        [SerializeField] private RectTransform _targetObject;
        private string _screenshotPath;
        private FileSaver fileSaver;

        private void Awake()
        {
            _dateText.text = DateTime.Now.ToString("dd.MM.yyyy");

            _webButton.onClick.AddListener(() => Application.OpenURL(ProgramUrl));
            _diplomaButton.onClick.AddListener(() => StartCoroutine(CaptureAndSavePDF()));

            LoadPlayerData();
        }

        private void Start()
        {
            fileSaver = gameObject.AddComponent<FileSaver>();
        }

        private void LoadPlayerData()
        {
            _nameText.text = PlayerPrefs.GetString("PlayerNickname");
            _practiceText.text = PlayerPrefs.GetString("CompletedPractice");
            _magoLegoText.text = PlayerPrefs.GetString("SelectedMagoLego");
        }

        private IEnumerator CaptureAndSavePDF()
        {
            yield return new WaitForEndOfFrame();

            Vector2 size = _targetObject.rect.size;
            Vector2 pivotOffset = new Vector2(size.x * _targetObject.pivot.x, size.y * _targetObject.pivot.y);
            
            Vector2 worldBottomLeft = (Vector2)_targetObject.position - pivotOffset;
            Vector2 screenBottomLeft = RectTransformUtility.WorldToScreenPoint(null, worldBottomLeft);

            Rect readRect = new Rect(screenBottomLeft.x, screenBottomLeft.y + 1, size.x, size.y - 1);

            Texture2D screenshot = new Texture2D((int)size.x, (int)size.y - 1, TextureFormat.RGB24, false);
            screenshot.ReadPixels(readRect, 0, 0);
            screenshot.Apply();

            byte[] bytes = screenshot.EncodeToPNG();

            fileSaver.SaveFile(bytes, "Diploma.png");
        }
    }
}