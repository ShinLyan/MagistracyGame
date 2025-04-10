using UnityEngine;
using UnityEngine.UI;

public class BlockClick : MonoBehaviour
{
    [SerializeField] private Button dialoguePanel;

    public void SwitchDialoguePanel()
    {
        dialoguePanel.enabled = true;
    }

    public void BlockClicks()
    {
        dialoguePanel.enabled = false;
    }
}
