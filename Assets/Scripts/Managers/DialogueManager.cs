using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject messagePanel;
    public TextMeshProUGUI messageText;

    private Queue<string> dialogueQueue;
    private bool isDialogueActive = false;

    public float timePerMessage = 2f;
    public float typingSpeed = 0.05f;

    private Coroutine typingCoroutine;
    public Image characterImage;


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

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(nextMessage));
    }

    private IEnumerator TypeText(string message)
    {
        messageText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            messageText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(timePerMessage);
        ShowNextMessage();
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        messagePanel.SetActive(false);
    }

    public bool IsDialogueActive => isDialogueActive;
}