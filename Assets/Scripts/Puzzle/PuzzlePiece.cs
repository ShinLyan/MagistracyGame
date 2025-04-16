using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePiece : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int _shapeID;
    public int ShapeID => _shapeID;

    public event System.Action<PuzzlePiece> OnPieceClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPieceClicked?.Invoke(this);
    }
}
