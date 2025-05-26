using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    [Header("Componentes")]
    public Image fullImage;
    public TextMeshProUGUI dialogueText;
    public RectTransform dialogueRect;
    public CanvasGroup fadePanel;

    [Header("Configurações")]
    public float fadeDuration = 1f;
    public float displayDuration = 4f;

    [TextArea]
    public string[] cutsceneTexts;

    private Vector2[] dialoguePositions;

    void Start()
    {
        dialoguePositions = new Vector2[]
        {
            new(-480, -270),
            new(480, -270),
            new(-480, 270),
            new(480, 270)
        };

        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        for (int i = 0; i < cutsceneTexts.Length; i++)
        {
            yield return StartCoroutine(FadeIn());

            dialogueText.text = cutsceneTexts[i];
            dialogueRect.anchoredPosition = dialoguePositions[i];

            yield return new WaitForSeconds(displayDuration);

            yield return StartCoroutine(FadeOut());
        }

        // Aqui você pode carregar a próxima cena ou desativar a cutscene
        // SceneManager.LoadScene("MainMenu");
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadePanel.alpha = 1f - (timer / fadeDuration);
            yield return null;
        }
        fadePanel.alpha = 0f;
    }

    IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadePanel.alpha = (timer / fadeDuration);
            yield return null;
        }
        fadePanel.alpha = 1f;
    }
}
