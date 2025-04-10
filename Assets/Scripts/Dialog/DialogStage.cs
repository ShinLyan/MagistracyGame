using UnityEngine;
using UnityEngine.UI;

public class DialogStage : MonoBehaviour
{
    /// <summary> Чёрный фон игрового поля.</summary>
    [SerializeField] private GameObject __blackBackgroundGameField;

    /// <summary> Чёрный фон.</summary> 
    [SerializeField] private Image _blackBackground;

    [SerializeField] private DialogManager _dialogManager;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _studentCardStage;

    /// <summary> Индекс сцены в диалоге.</summary>
    private int levelIndex;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /// <summary>При старте инициализируем обучение.</summary>
    private void Start() => InitializeDialogStage();

    /// <summary> Инициализировать руководство.</summary>
    private void InitializeDialogStage()
    {
        _blackBackground.enabled = true;
        levelIndex = 0;

    }

    /// <summary> Начать диалог.</summary>
    public void StartDialogue()
    {
        switch (levelIndex)
        {
            case 0:
                {
                    _mainMenu.SetActive(false);
                    _dialogManager.SwitchDialoguePanel(true);
                    __blackBackgroundGameField.SetActive(true);
                    break;
                }
            case 1: 
                {
                    _dialogManager.SwitchDialoguePanel(true);
                    __blackBackgroundGameField.SetActive(true);
                    _studentCardStage.SetActive(false);
                    break;
                }
        }
        _dialogManager.ContinueDialogue();
    }

    /// <summary> Закончить диалог.</summary>
    public void EndDialogue()
    {
        switch (levelIndex)
        {
            case 0:
                {
                    _dialogManager.SwitchDialoguePanel(false);
                    __blackBackgroundGameField.SetActive(false);
                    _studentCardStage.SetActive(true);
                    break;
                }
        }
        levelIndex += 1;

    }

    /// <summary> Добавить объект на тёмный фон.</summary>
    /// <param name="transform"> Объект, который необходимо добавить на тёмный фон.</param>
    /// <param name="worldPositionStays"> Если True, то параметры относительно родителя такие же, что и в мировом пространстве.
    /// Оставить ли позицию поля неизменным.</param>
    private void AddToBlackBackground(Transform transform, bool worldPositionStays = true)
    {
        transform.SetParent(__blackBackgroundGameField.GetComponent<Transform>(), worldPositionStays);
    }

    /// <summary> Убрать объект из тёмного фона.</summary>
    /// <param name="transform"> Объект, который необходимо убрать из тёмного фона.</param>
    /// <param name="place"> Место, куда необходимо поместить объект.</param>
    /// <param name="worldPositionStays"> Если True, то параметры относительно родителя такие же, что и в мировом пространстве.
    /// Оставить ли позицию поля неизменным.</param>
    private static void RemoveFromBlackBackground(Transform transform, Transform place, bool worldPositionStays = true)
    {
        transform.SetParent(place, worldPositionStays);
    }
}
