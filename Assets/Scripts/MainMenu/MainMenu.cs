using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MagistracyGame.Scripts.MainMenu.MainMenu

{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _returnButton;
        [SerializeField] private Image _Logos;
        [SerializeField] private string _gameSceneName;
        [SerializeField] private string _programUrl;

        private void Start()
        {
            SetupButtons();
        }

        private void SetupButtons()
        {
            _playButton.onClick.AddListener(StartGame);
            _returnButton.onClick.AddListener(ReturnToProgramPage);
        }

        private void StartGame()
        {
            SceneManager.LoadScene(_gameSceneName);
        }

        private void ReturnToProgramPage()
        {
            Application.OpenURL(_programUrl);
        }
    }
}