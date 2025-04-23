using System;
using System.Collections.Generic;
using UnityEngine;

public class NicknameFilter : MonoBehaviour
{
    private Dictionary<char, List<char>> replacementMap = new Dictionary<char, List<char>>()
    {
        { 'а', new List<char> { 'a', '@' } },
        { 'б', new List<char> { '6', 'b' } },
        { 'в', new List<char> { 'b', 'v' } },
        { 'г', new List<char> { 'r', 'g' } },
        { 'д', new List<char> { 'd' } },
        { 'е', new List<char> { 'e' } },
        { 'ё', new List<char> { 'e' } },
        { 'ж', new List<char> { '*', 'z' } },
        { 'з', new List<char> { '3', 'z' } },
        { 'и', new List<char> { 'u', 'i' } },
        { 'й', new List<char> { 'u', 'i' } },
        { 'к', new List<char> { 'k' } },
        { 'л', new List<char> { 'l' } },
        { 'м', new List<char> { 'm' } },
        { 'н', new List<char> { 'h', 'n' } },
        { 'о', new List<char> { 'o', '0' } },
        { 'п', new List<char> { 'n', 'p' } },
        { 'р', new List<char> { 'r', 'p' } },
        { 'с', new List<char> { 'c', 's' } },
        { 'т', new List<char> { 'm', 't' } },
        { 'у', new List<char> { 'y', 'u' } },
        { 'ф', new List<char> { 'f' } },
        { 'х', new List<char> { 'x', 'h' } },
        { 'ц', new List<char> { 'c' } },
        { 'ч', new List<char> { } },
        { 'ш', new List<char> { } },
        { 'щ', new List<char> { } },
        { 'ь', new List<char> { 'b' } },
        { 'ы', new List<char> { } },
        { 'ъ', new List<char> { } },
        { 'э', new List<char> { 'e' } },
        { 'ю', new List<char> { } },
        { 'я', new List<char> { } }
    };

    private List<string> bannedWords = new List<string>();

    void Start()
    {
        LoadBannedWords();
    }

    void LoadBannedWords()
    {
        TextAsset wordFile = Resources.Load<TextAsset>("banned_words");
        if (wordFile != null)
        {
            var lines = wordFile.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                bannedWords.Add(line.Trim().ToLower());
            }
        }
        else
        {
            Debug.LogError("Не удалось загрузить файл banned_words.txt из Resources.");
        }
    }

    public string Normalize(string input)
    {
        input = input.ToLower().Replace(" ", "");
        string result = "";

        foreach (char c in input)
        {
            bool replaced = false;
            foreach (var kv in replacementMap)
            {
                if (kv.Value.Contains(c))
                {
                    result += kv.Key;
                    replaced = true;
                    break;
                }
            }
            if (!replaced)
                result += c;
        }

        return result;
    }

    public int LevenshteinDistance(string a, string b)
    {
        int[,] dp = new int[a.Length + 1, b.Length + 1];

        for (int i = 0; i <= a.Length; i++) dp[i, 0] = i;
        for (int j = 0; j <= b.Length; j++) dp[0, j] = j;

        for (int i = 1; i <= a.Length; i++)
        {
            for (int j = 1; j <= b.Length; j++)
            {
                int cost = (a[i - 1] == b[j - 1]) ? 0 : 1;
                dp[i, j] = Math.Min(
                    Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                    dp[i - 1, j - 1] + cost
                );
            }
        }

        return dp[a.Length, b.Length];
    }

    public bool IsNicknameClean(string nickname)
    {
        string normalized = Normalize(nickname);
        foreach (string word in bannedWords)
        {
            for (int i = 0; i <= normalized.Length - word.Length; i++)
            {
                string fragment = normalized.Substring(i, word.Length);
                if (LevenshteinDistance(fragment, word) <= word.Length * 0.25)
                {
                    Debug.Log($"Обнаружено запрещённое слово: {word} в нике: {fragment}");
                    return false;
                }
            }
        }
        return true;
    }
}
