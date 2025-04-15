using System;
using UnityEngine;

namespace MagistracyGame.Quiz
{
    [Serializable]
    public class QuizQuestion
    {
        [field: SerializeField, TextArea] public string QuestionText { get; private set; }
        [field: SerializeField] public string[] Answers { get; private set; }
        [field: SerializeField] public int CorrectAnswerIndex { get; private set; }
        [field: SerializeField, TextArea] public string GuideTextCorrect { get; private set; }
        [field: SerializeField, TextArea] public string GuideTextIncorrect { get; private set; }
    }
}