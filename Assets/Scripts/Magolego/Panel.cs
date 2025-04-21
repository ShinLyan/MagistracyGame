using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _contentGroup; 
    [SerializeField] private Image _button;     
    [SerializeField] private Sprite _activePanelSprite;
    [SerializeField] private Sprite _normalPanelSprite;


    // включить/выключить выделение
    public void SetActive(bool active)
    {
        if (active)
        {
            _button.sprite = _activePanelSprite;
        } else
        {
            _button.sprite = _normalPanelSprite;
        }
        _contentGroup.alpha = active ? 1f : 0.6f;
    }
}
