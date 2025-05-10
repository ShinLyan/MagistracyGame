using System;
using System.Collections.Generic;
using UnityEngine;

public class NicknameFilter : MonoBehaviour
{
    private readonly Dictionary<char, List<char>> replacementMap = new()
    {
        { '�', new List<char> { 'a', '@' } },
        { '�', new List<char> { '6', 'b' } },
        { '�', new List<char> { 'b', 'v' } },
        { '�', new List<char> { 'r', 'g' } },
        { '�', new List<char> { 'd' } },
        { '�', new List<char> { 'e' } },
        { '�', new List<char> { 'e' } },
        { '�', new List<char> { '*', 'z' } },
        { '�', new List<char> { '3', 'z' } },
        { '�', new List<char> { 'u', 'i' } },
        { '�', new List<char> { 'u', 'i' } },
        { '�', new List<char> { 'k' } },
        { '�', new List<char> { 'l' } },
        { '�', new List<char> { 'm' } },
        { '�', new List<char> { 'h', 'n' } },
        { '�', new List<char> { 'o', '0' } },
        { '�', new List<char> { 'n', 'p' } },
        { '�', new List<char> { 'r', 'p' } },
        { '�', new List<char> { 'c', 's' } },
        { '�', new List<char> { 'm', 't' } },
        { '�', new List<char> { 'y', 'u' } },
        { '�', new List<char> { 'f' } },
        { '�', new List<char> { 'x', 'h' } },
        { '�', new List<char> { 'c' } },
        { '�', new List<char>() },
        { '�', new List<char>() },
        { '�', new List<char>() },
        { '�', new List<char> { 'b' } },
        { '�', new List<char>() },
        { '�', new List<char>() },
        { '�', new List<char> { 'e' } },
        { '�', new List<char>() },
        { '�', new List<char>() }
    };

    private readonly List<string> bannedWords = new();

    private void Start()
    {
        LoadBannedWords();
    }

    private void LoadBannedWords()
    {
        var wordFile = Resources.Load<TextAsset>("banned_words");
        if (wordFile != null)
        {
            string[] lines = wordFile.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines) bannedWords.Add(line.Trim().ToLower());
        }
        else
        {
            Debug.LogError("�� ������� ��������� ���� banned_words.txt �� Resources.");
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
                if (kv.Value.Contains(c))
                {
                    result += kv.Key;
                    replaced = true;
                    break;
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
        for (int j = 1; j <= b.Length; j++)
        {
            int cost = a[i - 1] == b[j - 1] ? 0 : 1;
            dp[i, j] = Math.Min(
                Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                dp[i - 1, j - 1] + cost
            );
        }

        return dp[a.Length, b.Length];
    }

    public bool IsNicknameClean(string nickname)
    {
        string normalized = Normalize(nickname);
        foreach (string word in bannedWords)
            for (int i = 0; i <= normalized.Length - word.Length; i++)
            {
                string fragment = normalized.Substring(i, word.Length);
                if (LevenshteinDistance(fragment, word) <= word.Length * 0.25)
                {
                    Debug.Log($"���������� ����������� �����: {word} � ����: {fragment}");
                    return false;
                }
            }

        return true;
    }
}