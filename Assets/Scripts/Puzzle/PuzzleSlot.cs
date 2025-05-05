using UnityEngine;
using UnityEngine.UI;

public class PuzzleSlot : MonoBehaviour
{
    [SerializeField] private int _requiredShapeID;
    [SerializeField] private GameObject _slotImage;
    [SerializeField] private GameObject _border;
    [SerializeField] private Sprite _orangeBorderSprite;
    [SerializeField] private Sprite _normalBorderSprite;

    public int RequiredShapeID => _requiredShapeID;

    public void Highlight(bool state)
    {
        if (state)
        {
            _border.transform.SetAsLastSibling();
            _border.GetComponent<Image>().sprite = _orangeBorderSprite;
        }
        else
        {
            _border.GetComponent<Image>().sprite = _normalBorderSprite;
        }
    }

    public void FillSlot()
    {
        var image = _slotImage.GetComponent<Image>();
        if (image != null) image.enabled = true;
        _border.SetActive(false);
    }

    public Image GetBorderImage()
    {
        return _border.GetComponent<Image>();
    }
}
