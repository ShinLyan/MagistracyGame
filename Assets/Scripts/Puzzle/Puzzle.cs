using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MagistracyGame.Puzzle
{
    public class DiplomaPuzzle : MonoBehaviour
    {
        [SerializeField] private Transform coverPage;
        [SerializeField] private Transform secondHalf;
        [SerializeField] private float _vibrationIntensity = 10f;
        [SerializeField] private float _fadeDuration = 1f;
        [SerializeField] private float _delayBeforeFlip = 0.5f;
        [SerializeField] private List<PuzzleSlot> _slots = new();
        [SerializeField] private List<PuzzlePiece> _pieces = new();
        [SerializeField] private Button _continueButton;
        [SerializeField] private GameObject _frontSide;
        [SerializeField] private GameObject _backSide;
        [SerializeField] private GameObject _texts;
        [SerializeField] private GameObject _piecesBar;
        [SerializeField] private Image _diplomaImage;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _practiceText;
        [SerializeField] private TMP_Text _magoLegoText;
        [SerializeField] private TMP_Text _dateText;

        private int _currentSlotIndex;
        private bool _isAnimating;

        private void Start()
        {
            _continueButton.gameObject.SetActive(false);

            HighlightCurrentSlot();
            LoadPlayerData();

            foreach (var piece in _pieces) piece.OnPieceClicked += HandlePieceClicked;
        }

        private void LoadPlayerData()
        {
            _nameText.text = PlayerPrefs.GetString("PlayerNickname");
            _practiceText.text = PlayerPrefs.GetString("CompletedPractice");
            _magoLegoText.text = PlayerPrefs.GetString("SelectedMagoLego");
        }

        private void OnDestroy()
        {
            foreach (var piece in _pieces.Where(piece => piece))
                piece.OnPieceClicked -= HandlePieceClicked;
        }

        private void HighlightCurrentSlot()
        {
            for (int i = 0; i < _slots.Count; i++) _slots[i].Highlight(i == _currentSlotIndex);
        }

        private void HandlePieceClicked(PuzzlePiece piece)
        {
            if (_isAnimating || _currentSlotIndex >= _slots.Count) return;

            var currentSlot = _slots[_currentSlotIndex];

            if (piece.ShapeID == currentSlot.RequiredShapeID)
                StartCoroutine(ProcessCorrectSelection(piece));
            else
                StartCoroutine(ProcessWrongSelection(piece));
        }

        private IEnumerator ProcessCorrectSelection(PuzzlePiece piece)
        {
            _isAnimating = true;

            piece.gameObject.SetActive(false);
            _slots[_currentSlotIndex].FillSlot();

            _currentSlotIndex++;

            if (_currentSlotIndex >= _slots.Count)
            {
                yield return StartCoroutine(CompletePuzzle());
            }
            else
            {
                yield return new WaitForSeconds(0.3f);
                HighlightCurrentSlot();
            }

            _isAnimating = false;
        }

        private IEnumerator ProcessWrongSelection(PuzzlePiece piece)
        {
            _isAnimating = true;
            var originalPosition = piece.transform.position;
            float timer = 0f;

            while (timer < 1f)
            {
                timer += Time.deltaTime;
                float intensity = (1f - timer) * _vibrationIntensity * 0.01f;
                piece.transform.position = originalPosition + new Vector3(
                    Mathf.Sin(timer * 30f) * intensity,
                    Mathf.Cos(timer * 20f) * intensity,
                    0f);
                yield return null;
            }

            piece.transform.position = originalPosition;
            _isAnimating = false;
        }

        private IEnumerator CompletePuzzle()
        {
            _dateText.text = DateTime.Now.ToString("dd.MM.yyyy");
            float fadeTimer = 0f;
            _piecesBar.gameObject.SetActive(false);
            while (fadeTimer < _fadeDuration)
            {
                fadeTimer += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, fadeTimer / _fadeDuration);
                var color = _diplomaImage.color;
                color.a = alpha;
                _diplomaImage.color = color;
                yield return null;
            }

            yield return new WaitForSeconds(_delayBeforeFlip);

            float flipDuration = 1f;
            var startRotation = Quaternion.Euler(0, 0, 0);
            var endRotation = Quaternion.Euler(0, -180, 0);

            var coverStartPosition = coverPage.localPosition;
            var halfStartPosition = secondHalf.localPosition;
            var coverEndPosition = new Vector3(0f, 0f, 0f);
            var halfEndPosition = new Vector3(337f, 0f, 0f);
            float flipTimer = 0f;
            bool swapped = false;
            while (flipTimer < flipDuration)
            {
                float t = flipTimer / flipDuration;
                t = Mathf.Clamp01(t);
                coverPage.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
                coverPage.localPosition = Vector3.Lerp(coverStartPosition, coverEndPosition, t);
                secondHalf.localPosition = Vector3.Lerp(halfStartPosition, halfEndPosition, t);
                if (!swapped && t >= 0.5f)
                {
                    _frontSide.SetActive(false);
                    _backSide.SetActive(true);
                    swapped = true;
                }

                flipTimer += Time.deltaTime;
                yield return null;
            }

            coverPage.localRotation = endRotation;
            secondHalf.localPosition = halfEndPosition;
            coverPage.localPosition = coverEndPosition;

            _isAnimating = false;
        }
    }
}