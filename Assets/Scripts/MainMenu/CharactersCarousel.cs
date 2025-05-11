using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharactersCarousel : MonoBehaviour
{
    public TextMeshProUGUI characterName;
    public Image characterImage;
    public Sprite[] charactersSprites;
    public string[] characterNames;
    private int currentIndex = 0;

    void Start()
    {
        currentIndex = PlayerPrefs.GetInt("characterIndex", 0);
        UpdateCharacterDisplay();
    }

    public void Next()
    {
        currentIndex = (currentIndex + 1) % charactersSprites.Length;
        UpdateCharacterDisplay();
    }

    public void Previous()
    {
        currentIndex = (currentIndex - 1 + charactersSprites.Length) % charactersSprites.Length;
        UpdateCharacterDisplay();
    }

    public void ConfirmSelection()
    {
        PlayerPrefs.SetInt("characterIndex", currentIndex);
        PlayerPrefs.SetString("CharacterName", characterNames[currentIndex]);
        PlayerPrefs.SetString("CharacterImage", charactersSprites[currentIndex].name);
        PlayerPrefs.Save();
    }

    void UpdateCharacterDisplay()
    {
        characterImage.sprite = charactersSprites[currentIndex];
        characterName.text = characterNames[currentIndex];
    }
}
