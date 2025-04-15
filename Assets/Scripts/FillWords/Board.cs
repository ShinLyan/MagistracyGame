using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace MagistracyGame.FillWords
{
    public class Board : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] private GameObject _wordsContainer;
        [SerializeField] private GameObject _continueButton;

        private Tile _startTile;
        private bool _isDragging;
        private int _foundWordsCount;
        private bool _isCompleted;

        private readonly HashSet<string> _foundWords = new();
        private List<Tile> _selectedTiles = new();
        private readonly List<Tile> _permanentSelection = new();
        private TextMeshProUGUI[] _wordsToGuess;
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
            "НЕЙРОСЕТЬ", "ПЛЮС", "СИНУС", "ДРОБЬ", "ЛОГАРИФМ", "МАТРИЦА", "ВЕКТОР"
        };

        private readonly Color[] _selectionColors =
        {
            new(0f, 0f, 1f), // Синий (0 слов)
            new(1f, 1f, 0f), // Желтый (1 слово)
            new(1f, 0f, 0f), // Красный (2 слова)
            new(0.5f, 0.5f, 0.5f), // Серый (3 слова)
            new(1f, 0.5f, 0f), // Оранжевый (4 слова)
            new(0.5f, 0f, 0.5f), // Фиолетовый (5 слов)
            new(0f, 1f, 0f) // Зеленый (6 слов)
        };

        private bool IsGameComplete => _foundWords.Count == _targetWords.Length;

        #endregion

        private void Awake()
        {
            _rows = GetComponentsInChildren<Row>();
            _wordsToGuess = _wordsContainer.GetComponentsInChildren<TextMeshProUGUI>();
            _continueButton.SetActive(false);
        }

        private void Start() => InitializeBoardLetters();

        private void InitializeBoardLetters()
        {
            for (int i = 0; i < _rows.Length; i++)
            {
                var tiles = _rows[i]._tiles;
                string rowLetters = _boardLetters[i].ToUpper();

                for (int j = 0; j < tiles.Length; j++)
                    tiles[j].SetLetter(rowLetters[j]);
            }
        }

        private void Update()
        {
            if (_isCompleted) return;

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
            foreach (var tile in row._tiles)
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

            int rowIndex = Array.FindIndex(_rows, row => Array.IndexOf(row._tiles, _startTile) != -1);
            int columnIndex = Array.IndexOf(_rows[rowIndex]._tiles, _startTile);

            if (isHorizontal)
                SelectHorizontal(rowIndex, columnIndex, direction.x);
            else
                SelectVertical(rowIndex, columnIndex, direction.y);

            _selectedTiles = _selectedTiles.Distinct().ToList();
            UpdateTileView();
        }

        private void SelectHorizontal(int rowIndex, int columnIndex, float x)
        {
            float tileWidth = _rows[0]._tiles[0].RectTransform.sizeDelta.x;
            int offset = Mathf.RoundToInt(x / tileWidth);
            int start = Mathf.Min(columnIndex, columnIndex + offset);
            int end = Mathf.Max(columnIndex, columnIndex + offset);

            for (int i = start; i <= end; i++)
                if (i >= 0 && i < _rows[rowIndex]._tiles.Length)
                    _selectedTiles.Add(_rows[rowIndex]._tiles[i]);
        }

        private void SelectVertical(int rowIndex, int columnIndex, float y)
        {
            float tileHeight = _rows[0]._tiles[0].RectTransform.sizeDelta.y;
            int offset = Mathf.RoundToInt(y / tileHeight);
            int start = Mathf.Min(rowIndex, rowIndex - offset);
            int end = Mathf.Max(rowIndex, rowIndex - offset);

            for (int i = start; i <= end; i++)
                if (i >= 0 && i < _rows.Length)
                    _selectedTiles.Add(_rows[i]._tiles[columnIndex]);
        }

        private void UpdateTileView()
        {
            var currentColor = GetCurrentColor();

            foreach (var row in _rows)
            foreach (var tile in row._tiles)
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

            if (IsGameComplete) CompleteGame();
        }

        private void StrikeThroughWord(string word)
        {
            foreach (var wordText in _wordsToGuess)
                if (string.Equals(wordText.text, word, StringComparison.CurrentCultureIgnoreCase))
                {
                    wordText.text = $"<s>{wordText.text}</s>";
                    wordText.color = Color.gray;
                    break;
                }
        }

        public void CompleteGame()
        {
            _isCompleted = true;
            _continueButton.SetActive(true);
        }
    }
}