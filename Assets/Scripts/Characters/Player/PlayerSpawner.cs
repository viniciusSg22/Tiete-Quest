using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    void Start()
    {
        string selectedCharacter = PlayerPrefs.GetString("CharacterName");

        switch (selectedCharacter)
        {
            case "Advogado":
                selectedCharacter = "lawyer";
                break;
            case "Agrônomo":
                selectedCharacter = "agronomist";
                break;
            case "Bióloga":
                selectedCharacter = "biologist";
                break;
            case "Engenheiro":
                selectedCharacter = "engineer";
                break;
        }

        GameObject playerPrefab = Resources.Load<GameObject>($"Characters/{selectedCharacter}/{selectedCharacter}");

        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError($"Prefab do personagem '{selectedCharacter}' não encontrado.");
        }
    }
}
