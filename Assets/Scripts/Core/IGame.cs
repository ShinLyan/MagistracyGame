using UnityEngine.Events;

namespace MagistracyGame.Core
{
    public interface IGame
    {
        bool IsGameFinished { get; }

        UnityEvent OnGameFinished { get; }

        void FinishGame();
    }
}