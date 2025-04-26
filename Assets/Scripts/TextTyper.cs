using System.Collections;
using TMPro;
using UnityEngine;

public class TextTyper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void TypeText(string text, float charactersPerSecond)
    {
        StopAllCoroutines();
        StartCoroutine(TypeTextCoroutine(text, charactersPerSecond));
    }

    private IEnumerator TypeTextCoroutine(string text, float charactersPerSecond)
    {
        _text.text = "";
        float delay = 1f / charactersPerSecond;

        foreach (char c in text)
        {
            _text.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
}