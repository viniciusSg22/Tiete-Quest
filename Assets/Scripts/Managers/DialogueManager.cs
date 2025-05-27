using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject messagePanel;
    public TextMeshProUGUI messageText;

    private Queue<string> dialogueQueue;
    private bool isDialogueActive = false;

    public float timePerMessage = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            dialogueQueue = new Queue<string>();
        }
        else
        {
            Destroy(gameObject);
        }

        if (messagePanel != null) messagePanel.SetActive(false);
    }

    public void StartDialogue(IEnumerable<string> messages)
    {
        if (messageText == null || messagePanel == null) return;

        dialogueQueue.Clear();
        foreach (var message in messages) dialogueQueue.Enqueue(message);

        messagePanel.SetActive(true);
        isDialogueActive = true;

        ShowNextMessage();
    }

    private void ShowNextMessage()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string nextMessage = dialogueQueue.Dequeue();
        messageText.text = nextMessage;

        CancelInvoke(nameof(ShowNextMessage));
        Invoke(nameof(ShowNextMessage), timePerMessage);
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        messagePanel.SetActive(false);
    }

    public bool IsDialogueActive => isDialogueActive;
}
