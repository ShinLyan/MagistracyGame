using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MagistracyGame.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _returnButton;
        [SerializeField] private string _gameSceneName;

        private const string ProgramUrl = "https://www.hse.ru/ma/gamedev/";

        private void Awake() => SetupButtons();

        private void SetupButtons()
        {
            _playButton.onClick.AddListener(StartGame);
            _returnButton.onClick.AddListener(ReturnToProgramPage);
        }

        private void StartGame() => SceneManager.LoadScene(_gameSceneName);

        private static void ReturnToProgramPage() => Application.OpenURL(ProgramUrl);
    }
}