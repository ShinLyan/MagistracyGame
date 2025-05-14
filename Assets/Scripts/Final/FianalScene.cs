using MagistracyGame.UI;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;





public class FianalScene : MonoBehaviour
{
    [SerializeField] private TMP_Text _dateText;
    [SerializeField] private Button _diplomaButton;
    [SerializeField] private Button _webButton;
    [SerializeField] private Camera _diplomaCamera;


    private const string MagoLegoUrl = "https://electives.hse.ru/mg_oi/";

    private string screenshotPath;

    void Awake()
    {
        _dateText.text = DateTime.Now.ToString("dd.MM.yyyy");

        _webButton.onClick.AddListener(() => Application.OpenURL(MagoLegoUrl));
        _diplomaButton.onClick.AddListener(() => StartCoroutine(CaptureAndSavePDF()));
        string diplomaDirectory = Path.Combine(Application.persistentDataPath, "Diplomas");
        if (!Directory.Exists(diplomaDirectory))
        {
            Directory.CreateDirectory(diplomaDirectory);
        }

        screenshotPath = Path.Combine(diplomaDirectory, "DiplomaScreenshot.png");
    }

    

    IEnumerator CaptureAndSavePDF()
    {
        yield return new WaitForEndOfFrame();

        // Сохранение скриншота
        RenderTexture renderTexture = new RenderTexture(1024, 1024, 24);
        _diplomaCamera.targetTexture = renderTexture;

        // Захват изображения
        Texture2D screenshot = new Texture2D(1024, 1024, TextureFormat.RGB24, false);
        _diplomaCamera.Render();
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(new Rect(0, 0, 1024, 1024), 0, 0);
        screenshot.Apply();
        byte[] bytes = screenshot.EncodeToPNG();
        File.WriteAllBytes(screenshotPath, bytes);

        // Очистка
        _diplomaCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);
        Destroy(screenshot);

        Debug.Log("Скриншот сохранён по пути: " + screenshotPath);

    }
}
