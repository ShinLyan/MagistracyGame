using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DiplomaPuzzle : MonoBehaviour
{
    [SerializeField] private float _highlightThickness = 5f;
    [SerializeField] private float _vibrationIntensity = 10f;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private float _flipDuration = 1f;
    [SerializeField] private float _delayBeforeFlip = 0.5f;
    [SerializeField] private List<PuzzleSlot> _slots = new List<PuzzleSlot>();
    [SerializeField] private List<PuzzlePiece> _pieces = new List<PuzzlePiece>();
    [SerializeField] private Button _continueButton;
    [SerializeField] private GameObject _frontSide;
    [SerializeField] private GameObject _texts;
    [SerializeField] private GameObject _piecesBar;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _practiceText;
    [SerializeField] private TMP_Text _magoLegoText;

    private int _currentSlotIndex = 0;
    private bool _isAnimating = false;

    private void Start()
    {
        _continueButton.gameObject.SetActive(false);

        HighlightCurrentSlot();
        LoadPlayerData();

        foreach (PuzzlePiece piece in _pieces)
        {
            piece.OnPieceClicked += HandlePieceClicked;
        }
    }

    private void LoadPlayerData()
    {
        _nameText.text = PlayerPrefs.GetString("PlayerNickname", "Студент");
        _practiceText.text = PlayerPrefs.GetString("CompletedPractice", "Практика в ЦИКИ");
        _magoLegoText.text = PlayerPrefs.GetString("SelectedMagoLego", "МагоЛего: Data Science");
    }

    private void OnDestroy()
    {
        foreach (PuzzlePiece piece in _pieces)
        {
            if (piece != null)
            {
                piece.OnPieceClicked -= HandlePieceClicked;
            }
        }
    }

    private void HighlightCurrentSlot()
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            _slots[i].Highlight(i == _currentSlotIndex, _highlightThickness);
        }
    }

    private void HandlePieceClicked(PuzzlePiece piece)
    {
        if (_isAnimating || _currentSlotIndex >= _slots.Count) return;

        PuzzleSlot currentSlot = _slots[_currentSlotIndex];

        if (piece.ShapeID == currentSlot.RequiredShapeID)
        {
            StartCoroutine(ProcessCorrectSelection(piece));
        }
        else
        {
            StartCoroutine(ProcessWrongSelection(piece));
        }
    }

    private void SetChildrenActiveState(bool active)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
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
        Vector3 originalPosition = piece.transform.position;
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
        float fadeTimer = 0f;
        List<Image> borders = new List<Image>();

        foreach (var slot in _slots)
        {
            borders.Add(slot.GetBorderImage());
        }

        while (fadeTimer < _fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, fadeTimer / _fadeDuration);

            foreach (var border in borders)
            {
                Color color = border.color;
                color.a = alpha;
                border.color = color;
            }
            yield return null;
        }

        yield return new WaitForSeconds(_delayBeforeFlip);

        float flipTimer = 0f;

        while (flipTimer < _flipDuration / 2)
        {
            flipTimer += Time.deltaTime;
            float angle = Mathf.Lerp(0f, 90f, flipTimer / _flipDuration * 2);         
            _frontSide.transform.rotation = Quaternion.Euler(0, angle, 0);
            yield return null;
        }
        yield return new WaitForSeconds(_flipDuration / 2);

        SetChildrenActiveState(false);
        _texts.SetActive(true);

        _frontSide.transform.rotation = Quaternion.Euler(0, -90f, 0);

        flipTimer = 0f;

        while (flipTimer < _flipDuration / 2)
        {
            flipTimer += Time.deltaTime;
            float angle = Mathf.Lerp(-90f, 0f, flipTimer / _flipDuration * 2);
            _frontSide.transform.rotation = Quaternion.Euler(0, angle, 0);
            yield return null;
        }
        
        _continueButton.gameObject.SetActive(true);
        _piecesBar.gameObject.SetActive(false);
        _isAnimating = false;
    }
}