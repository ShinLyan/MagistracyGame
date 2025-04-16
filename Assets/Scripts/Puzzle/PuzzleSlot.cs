using UnityEngine;
using UnityEngine.UI;

public class PuzzleSlot : MonoBehaviour
{
    [SerializeField] private int _requiredShapeID;
    [SerializeField] private GameObject _slotImage;
    [SerializeField] private GameObject _border;

    public int RequiredShapeID => _requiredShapeID;

    public void Highlight(bool state, float thickness)
    {
        if (!_border.TryGetComponent<Outline>(out var outline)) outline = _border.AddComponent<Outline>();

        outline.enabled = state;

        if (state)
        {
            outline.effectDistance = new Vector2(thickness, thickness);
            outline.effectColor = Color.yellow;
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
