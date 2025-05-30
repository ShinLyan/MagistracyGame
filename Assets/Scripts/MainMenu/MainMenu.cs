using UnityEngine;
using UnityEngine.UI;

namespace MagistracyGame.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _returnButton;

        private const string ProgramUrl = "https://www.hse.ru/ma/gamedev/";

        private void Awake()
        {
            _returnButton.onClick.AddListener(ReturnToProgramPage);
        }

        private static void ReturnToProgramPage() => Application.OpenURL(ProgramUrl);
    }
}