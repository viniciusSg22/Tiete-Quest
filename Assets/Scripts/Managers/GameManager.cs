using UnityEngine;
using Photon.Pun;
using WebSocketSharp;

public class GameManager : MonoBehaviourPunCallbacks
{
    public Transform[] spawnPoints;

    private void Start()
    {
        SpawnPlayerCharacter();
    }

    private void SpawnPlayerCharacter()
    {
        string characterName = (string)PhotonNetwork.LocalPlayer.CustomProperties["CharacterName"];

        switch (characterName)
        {
            case "Advogado":
                characterName = "Lawyer";
                break;
            case "Agrônomo":
                characterName = "Agronomist";
                break;
            case "Bióloga":
                characterName = "Biologist";
                break;
            case "Engenheiro":
                characterName = "Engineer";
                break;
        }

        GameObject characterPrefab = Resources.Load<GameObject>($"Characters/{characterName}/{characterName}");

        if (characterPrefab == null)
        {
            characterPrefab = Resources.Load<GameObject>($"Characters/Biologist/Biologist");
            // Debug.LogError("Personagem não encontrado em Resources/Characters/" + characterName);
            // return;
        }

        int index = System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);
        Transform spawnPoint = spawnPoints[index % spawnPoints.Length];

        if (characterName.IsNullOrEmpty()) characterName = "Biologist";
        GameObject playerInstance = PhotonNetwork.Instantiate($"Characters/{characterName}/{characterName}", spawnPoint.position, spawnPoint.rotation);

        PhotonView view = playerInstance.GetComponent<PhotonView>();

        if (view != null && view.IsMine)
        {
            CameraFollow cameraFollow = FindFirstObjectByType<CameraFollow>();
            if (cameraFollow != null)
            {
                cameraFollow.target = playerInstance.transform;
            }
            else
            {
                Debug.LogWarning("CameraFollow não encontrado na cena!");
            }
        }
        else
        {
            Debug.Log("Não foi possivel instanciar o jogador no photon!");
        }
    }
}