using System;
using System.Collections.Generic;
using System.Linq;
using MagistracyGame.Core;
using UnityEngine;
using UnityEngine.Events;

namespace MagistracyGame.FillWords
{
    public class FillWords : MonoBehaviour, IGame
    {
        #region Fields and Properties

        [SerializeField] private GameObject _startPanel;
        [SerializeField] private Transform _wordsContainer;
        [SerializeField] private Word _wordPrefab;

        private Tile _startTile;
        private bool _isDragging;
        private int _foundWordsCount;

        private readonly HashSet<string> _foundWords = new();
        private List<Tile> _selectedTiles = new();
        private readonly List<Tile> _permanentSelection = new();
        private List<Word> _wordsToGuess;
        private Row[] _rows;

        private readonly string[] _boardLetters =
        {
            "НЕЙРОСЕТЬУКВ",
            "ГНАУКВУЙУИТЕ",
            "ПЕСВВНДРОБЬК",
            "ЛУИЫСАММАУКТ",
            "ЮКНФЫСФФФЫФО",
            "СГУЛОГАРИФМР",
            "ГВСМАТРИЦАИМ"
        };

        private readonly string[] _targetWords =
        {
            "МАТРИЦА", "НЕЙРОСЕТЬ", "ВЕКТОР", "СИНУС", "ПЛЮС", "ДРОБЬ", "ЛОГАРИФМ"
        };

        private readonly Color[] _selectionColors =
        {
            new(0.988f, 0.424f, 0.729f), // #FC6CBA
            new(0.875f, 0.161f, 0.318f), // #DF2951
            new(1.0f, 0.427f, 0.0f), // #FF6D00
            new(0.176f, 0.757f, 0.176f), // #2DC12D
            new(0.024f, 0.702f, 0.953f), // #06B3F3
            new(0.016f, 0.314f, 0.733f), // #0450BB
            new(0.455f, 0.259f, 0.89f) // #7442E3
        };

        private bool IsGameComplete => _foundWords.Count == _targetWords.Length;

        #endregion

        private void Awake()
        {
            _rows = GetComponentsInChildren<Row>();
            _wordsToGuess = new List<Word>();
            foreach (Transform child in _wordsContainer) Destroy(child.gameObject);
            foreach (string word in _targetWords)
            {
                var instance = Instantiate(_wordPrefab, _wordsContainer);
                instance.SetWord(word);
                _wordsToGuess.Add(instance);
            }
        }

        private void Start()
        {
            InitializeBoardLetters();
            _startPanel.SetActive(true);
        }

        private void InitializeBoardLetters()
        {
            for (int i = 0; i < _rows.Length; i++)
            {
                var tiles = _rows[i].Tiles;
                string rowLetters = _boardLetters[i].ToUpper();

                for (int j = 0; j < tiles.Length; j++)
                    tiles[j].SetLetter(rowLetters[j]);
            }
        }

        private void Update()
        {
            if (IsGameFinished || _startPanel.activeSelf) return;

            HandleInput();
        }

        private void HandleInput()
        {
            Vector2 mousePosition = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                _startTile = GetTileAtPosition(mousePosition);
                if (!_startTile || _permanentSelection.Contains(_startTile)) return;

                _isDragging = true;
                _selectedTiles.Clear();
                UpdateSelection(mousePosition);
            }

            if (!_isDragging) return;

            if (Input.GetMouseButton(0)) UpdateSelection(mousePosition);

            if (!Input.GetMouseButtonUp(0)) return;

            _isDragging = false;
            if (TryGetMatchedWord(out string word)) ProcessMatchedWord(word);

            _selectedTiles.Clear();
            UpdateTileView();
        }

        private Tile GetTileAtPosition(Vector2 position)
        {
            foreach (var row in _rows)
            foreach (var tile in row.Tiles)
            {
                var tilePosition = tile.GetPosition();
                var size = tile.RectTransform.sizeDelta;
                if (position.x >= tilePosition.x - size.x / 2 && position.x <= tilePosition.x + size.x / 2 &&
                    position.y >= tilePosition.y - size.y / 2 && position.y <= tilePosition.y + size.y / 2)
                    return tile;
            }

            return null;
        }

        private void UpdateSelection(Vector2 currentPosition)
        {
            if (!_startTile) return;

            _selectedTiles.Clear();

            var direction = currentPosition - _startTile.GetPosition();
            bool isHorizontal = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);

            int rowIndex = Array.FindIndex(_rows, row => Array.IndexOf(row.Tiles, _startTile) != -1);
            int columnIndex = Array.IndexOf(_rows[rowIndex].Tiles, _startTile);

            if (isHorizontal)
                SelectHorizontal(rowIndex, columnIndex, direction.x);
            else
                SelectVertical(rowIndex, columnIndex, direction.y);

            _selectedTiles = _selectedTiles.Distinct().ToList();
            UpdateTileView();
        }

        private void SelectHorizontal(int rowIndex, int columnIndex, float x)
        {
            const float HorizontalSpacing = 20f;

            float tileWidth = _rows[0].Tiles[0].RectTransform.sizeDelta.x + HorizontalSpacing;
            int offset = Mathf.RoundToInt(x / tileWidth);
            int start = Mathf.Min(columnIndex, columnIndex + offset);
            int end = Mathf.Max(columnIndex, columnIndex + offset);

            for (int i = start; i <= end; i++)
                if (i >= 0 && i < _rows[rowIndex].Tiles.Length)
                    _selectedTiles.Add(_rows[rowIndex].Tiles[i]);
        }

        private void SelectVertical(int rowIndex, int columnIndex, float y)
        {
            const float VerticalSpacing = 20f;

            float tileHeight = _rows[0].Tiles[0].RectTransform.sizeDelta.y + VerticalSpacing;
            int offset = Mathf.RoundToInt(y / tileHeight);
            int start = Mathf.Min(rowIndex, rowIndex - offset);
            int end = Mathf.Max(rowIndex, rowIndex - offset);

            for (int i = start; i <= end; i++)
                if (i >= 0 && i < _rows.Length)
                    _selectedTiles.Add(_rows[i].Tiles[columnIndex]);
        }

        private void UpdateTileView()
        {
            var currentColor = GetCurrentColor();

            foreach (var row in _rows)
            foreach (var tile in row.Tiles)
            {
                if (_permanentSelection.Contains(tile)) continue;

                bool isSelected = _selectedTiles.Contains(tile);
                tile.SetSelected(isSelected, isSelected ? currentColor : Color.white);
            }
        }

        private Color GetCurrentColor()
        {
            int colorIndex = Mathf.Min(_foundWordsCount, _selectionColors.Length - 1);
            return _selectionColors[colorIndex];
        }

        private bool TryGetMatchedWord(out string matchedWord)
        {
            string forward = string.Concat(_selectedTiles.Select(t => t.Letter));
            string reversed = new(forward.Reverse().ToArray());

            foreach (string target in _targetWords)
                if ((target == forward || target == reversed) && !_foundWords.Contains(target))
                {
                    matchedWord = target;
                    return true;
                }

            matchedWord = null;
            return false;
        }

        private void ProcessMatchedWord(string word)
        {
            _foundWords.Add(word);
            _foundWordsCount++;
            _permanentSelection.AddRange(_selectedTiles);
            StrikeThroughWord(word);

            if (IsGameComplete) FinishGame();
        }

        private void StrikeThroughWord(string word)
        {
            for (int i = 0; i < _targetWords.Length; i++)
                if (string.Equals(word, _targetWords[i]))
                {
                    _wordsToGuess[i].MarkAsFound();
                    break;
                }
        }

        #region IGame

        public bool IsGameFinished { get; private set; }

        [field: SerializeField] public UnityEvent OnGameFinished { get; private set; }

        public void FinishGame()
        {
            IsGameFinished = true;
            OnGameFinished?.Invoke();
        }

        #endregion
    }
}