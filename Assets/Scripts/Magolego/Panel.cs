using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    [SerializeField] private Sprite _activePanelSprite;
    [SerializeField] private Sprite _normalPanelSprite;

    private Image _panelImage;

    private void Awake() => _panelImage = GetComponent<Image>();

    public void SetActive(bool isActive) => _panelImage.sprite = isActive ? _activePanelSprite : _normalPanelSprite;
}