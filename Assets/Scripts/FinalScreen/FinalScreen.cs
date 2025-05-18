using System;
using System.Collections;
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
        [SerializeField] private RectTransform _targetObject;

        private const string ProgramUrl = "https://www.hse.ru/ma/gamedev/";
        private string _screenshotPath;
        private FileSaver _fileSaver;

        private void Awake()
        {
            _dateText.text = DateTime.Now.ToString("dd.MM.yyyy");

            _webButton.onClick.AddListener(() => Application.OpenURL(ProgramUrl));
            _diplomaButton.onClick.AddListener(() => StartCoroutine(CaptureAndSavePDF()));

            LoadPlayerData();
        }

        private void Start()
        {
            _fileSaver = gameObject.AddComponent<FileSaver>();
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

            var size = _targetObject.rect.size;
            var pivotOffset = new Vector2(size.x * _targetObject.pivot.x, size.y * _targetObject.pivot.y);

            var worldBottomLeft = (Vector2)_targetObject.position - pivotOffset;
            var screenBottomLeft = RectTransformUtility.WorldToScreenPoint(null, worldBottomLeft);

            var readRect = new Rect(screenBottomLeft.x, screenBottomLeft.y + 1, size.x, size.y - 1);

            var screenshot = new Texture2D((int)size.x, (int)size.y - 1, TextureFormat.RGB24, false);
            screenshot.ReadPixels(readRect, 0, 0);
            screenshot.Apply();

            byte[] bytes = screenshot.EncodeToPNG();

            _fileSaver.SaveFile(bytes, "Diploma.png");
        }
    }
}