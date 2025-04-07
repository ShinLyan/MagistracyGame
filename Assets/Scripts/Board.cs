using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class Board : MonoBehaviour
{
    [SerializeField] private string[] boardLetters;
    [SerializeField] private string[] targetWords = new string[] {};
    [SerializeField] private GameObject wordsObject;
    
    private Tile startTile;
    private bool isDragging = false;
    private int foundWordsCount = 0;
    private Color[] selectionColors = new Color[]
    {
        new Color(0f, 0f, 1f),        // Синий (0 слов)
        new Color(1f, 1f, 0f),        // Желтый (1 слово)
        new Color(1f, 0f, 0f),        // Красный (2 слова)
        new Color(0.5f, 0.5f, 0.5f),  // Серый (3 слова)
        new Color(1f, 0.5f, 0f),      // Оранжевый (4 слова)
        new Color(0.5f, 0f, 0.5f),    // Фиолетовый (5 слов)
        new Color(0f, 1f, 0f)         // Зеленый (6 слов)
    };
    private HashSet<string> foundWords = new HashSet<string>();
    private List<Tile> currentSelection = new List<Tile>();
    private List<Tile> permanentSelection = new List<Tile>();
    private List<TextMeshProUGUI> wordsToGuess = new List<TextMeshProUGUI>();
    
    public Row[] rows;

    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();

        if (wordsObject != null)
        {
            wordsToGuess = wordsObject.GetComponentsInChildren<TextMeshProUGUI>().ToList();
        }
    }

    private void Start()
    {
        InitializeBoardLetters();
    }

    private void InitializeBoardLetters()
    {
        if (boardLetters == null || boardLetters.Length == 0)
        {
            return;
        }

        if (boardLetters.Length != rows.Length)
        {
            return;
        }

        for (int row = 0; row < rows.Length; row++)
        {
            Tile[] tiles = rows[row].tiles;
            string rowLetters = boardLetters[row].ToUpper();

            if (rowLetters.Length != tiles.Length)
            {
                continue;
            }

            for (int col = 0; col < tiles.Length; col++)
            {
                if (rowLetters[col] != ' ')
                {
                    tiles[col].SetLetter(rowLetters[col]);
                }
            }
        }
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        Vector2 mousePosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            startTile = GetTileAtPosition(mousePosition);
            if (startTile != null && !permanentSelection.Contains(startTile))
            {
                isDragging = true;
                currentSelection.Clear();
                UpdateSelection(mousePosition);
            }
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            UpdateSelection(mousePosition);
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            CheckWord();
            currentSelection.Clear();
            UpdateTileVisuals();
        }
    }

    private Tile GetTileAtPosition(Vector2 position)
    {
        foreach (Row row in rows)
        {
            foreach (Tile tile in row.tiles)
            {
                Vector2 tilePos = tile.GetPosition();
                RectTransform rt = tile.GetComponent<RectTransform>();
                Vector2 size = rt.sizeDelta;

                if (position.x >= tilePos.x - size.x/2 && position.x <= tilePos.x + size.x/2 &&
                    position.y >= tilePos.y - size.y/2 && position.y <= tilePos.y + size.y/2)
                {
                    return tile;
                }
            }
        }
        return null;
    }

    private void UpdateSelection(Vector2 currentPosition)
    {
        if (startTile == null) return;

        currentSelection.Clear();
        
        Vector2 startPos = startTile.GetPosition();
        Vector2 direction = (currentPosition - startPos);
        bool isHorizontal = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);
        
        int startRow = Array.FindIndex(rows, r => Array.IndexOf(r.tiles, startTile) != -1);
        int startCol = Array.IndexOf(rows[startRow].tiles, startTile);

        if (isHorizontal)
        {
            // Используем точное расстояние в пикселях, а не округление
            float tileWidth = rows[0].tiles[0].GetComponent<RectTransform>().sizeDelta.x;
            float distancePixels = direction.x / tileWidth;
            int distance = Mathf.RoundToInt(distancePixels);
            int minCol = Mathf.Min(startCol, startCol + distance);
            int maxCol = Mathf.Max(startCol, startCol + distance);

            if (distance >= 0) // Вправо
            {
                for (int col = startCol; col <= maxCol; col++)
                {
                    if (col >= 0 && col < rows[startRow].tiles.Length)
                    {
                        currentSelection.Add(rows[startRow].tiles[col]);
                    }
                }
            }
            else // Влево
            {
                for (int col = startCol; col >= minCol; col--)
                {
                    if (col >= 0 && col < rows[startRow].tiles.Length)
                    {
                        currentSelection.Add(rows[startRow].tiles[col]);
                    }
                }
            }
        }
        else
        {
            // Используем точное расстояние в пикселях для вертикального выделения
            float tileHeight = rows[0].tiles[0].GetComponent<RectTransform>().sizeDelta.y;
            float distancePixels = direction.y / tileHeight;
            int distance = Mathf.RoundToInt(distancePixels);
            int endRow = startRow - distance;

            if (endRow <= startRow) // Вверх
            {
                for (int row = startRow; row >= endRow; row--)
                {
                    if (row >= 0 && row < rows.Length)
                    {
                        currentSelection.Add(rows[row].tiles[startCol]);
                    }
                }
            }
            else // Вниз
            {
                for (int row = startRow; row <= endRow; row++)
                {
                    if (row >= 0 && row < rows.Length)
                    {
                        currentSelection.Add(rows[row].tiles[startCol]);
                    }
                }
            }
        }

        currentSelection = currentSelection.Distinct().ToList();
        UpdateTileVisuals();
    }

    private void UpdateTileVisuals()
    {
        foreach (Row row in rows)
        {
            foreach (Tile tile in row.tiles)
            {
                if (!permanentSelection.Contains(tile))
                {
                    tile.SetSelected(false, Color.white);
                }
            }
        }

        Color currentColor = GetCurrentColor();
        foreach (Tile tile in currentSelection)
        {
            if (!permanentSelection.Contains(tile))
            {
                tile.SetSelected(true, currentColor);
            }
        }
    }

    private void CheckWord()
    {
        string selectedWord = string.Join("", currentSelection.Select(t => t.letter));
        string reversedWord = string.Join("", currentSelection.Reverse<Tile>().Select(t => t.letter));

        foreach (string target in targetWords)
        {
            if ((selectedWord == target || reversedWord == target) && !foundWords.Contains(target))
            {
                foundWords.Add(target);
                foundWordsCount++;
                permanentSelection.AddRange(currentSelection);
                StrikeThroughWord(target);

                if (IsGameComplete())
                {
                    Debug.Log("Congratulations! All words have been found! Ready to load the next scene.");
                }
                break;
            }
        }
    }

    private void StrikeThroughWord(string word)
    {
        foreach (var wordText in wordsToGuess)
        {
            if (wordText.text.ToUpper() == word.ToUpper())
            {
                wordText.text = $"<s>{wordText.text}</s>";
                break;
            }
        }
    }

    private Color GetCurrentColor()
    {
        int colorIndex = Mathf.Min(foundWordsCount, selectionColors.Length - 1);
        return selectionColors[colorIndex];
    }

    public bool IsGameComplete()
    {
        return foundWords.Count == targetWords.Length;
    }
}