using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MagistracyGame.UI
{
    [RequireComponent(typeof(Button), typeof(Image))]
    public class UIButton : MonoBehaviour
    {
        [SerializeField] private bool _isInteractable = true;
        [SerializeField] private TextMeshProUGUI _label;

        [Header("Sprites")]
        [SerializeField] private Sprite _activeBackground;

        [SerializeField] private Sprite _disabledBackground;

        public Button Button { get; private set; }

        private Image _backgroundImage;
        private readonly Color ActiveTextColor = Color.white;
        private readonly Color DisabledTextColor = new Color32(153, 153, 153, 255);

        private void Awake()
        {
            Button = GetComponent<Button>();
        }

        private void OnValidate() => ApplyState();

        private void ApplyState()
        {
            Button = GetComponent<Button>();
            Button.interactable = _isInteractable;

            _backgroundImage = GetComponent<Image>();
            _backgroundImage.sprite = _isInteractable ? _activeBackground : _disabledBackground;

            _label.color = _isInteractable ? ActiveTextColor : DisabledTextColor;
        }

        public void SetInteractable(bool value)
        {
            _isInteractable = value;
            ApplyState();
        }
    }
}