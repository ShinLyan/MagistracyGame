using UnityEngine;

namespace MagistracyGame.Quiz
{
    [CreateAssetMenu(menuName = "MagistracyGame/QuizData")]
    public class QuizData : ScriptableObject
    {
        [field: SerializeField] public QuizQuestion[] Questions { get; private set; }
    }
}