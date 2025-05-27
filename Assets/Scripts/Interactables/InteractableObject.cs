using UnityEngine;
using System.Collections.Generic;

public class InteractableObject : MonoBehaviour, IInteractable
{
    public bool IsInteracted { get; private set; }

    [TextArea]
    public List<string> dialogueLines = new();

    public bool CanInteract()
    {
        return !IsInteracted && !DialogueManager.Instance.IsDialogueActive;
    }

    public void Interact()
    {
        if (!CanInteract()) return;

        IsInteracted = true;

        if (DialogueManager.Instance != null) DialogueManager.Instance.StartDialogue(dialogueLines);
    }
}
