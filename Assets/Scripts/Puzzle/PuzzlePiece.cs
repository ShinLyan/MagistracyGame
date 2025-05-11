using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MagistracyGame.Puzzle
{
    public class PuzzlePiece : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private int _shapeID;
        public int ShapeID => _shapeID;

        public event Action<PuzzlePiece> OnPieceClicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnPieceClicked?.Invoke(this);
        }
    }
}