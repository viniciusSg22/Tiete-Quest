using UnityEngine;
using Photon.Pun;
using WebSocketSharp;

public class GameManager : MonoBehaviourPunCallbacks
{
    public Transform SpawnPoint;

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

        if (characterName.IsNullOrEmpty()) characterName = "Biologist";
        GameObject playerInstance = PhotonNetwork.Instantiate($"Characters/{characterName}/{characterName}", SpawnPoint.position, SpawnPoint.rotation);

        PhotonView view = playerInstance.GetComponent<PhotonView>();

        if (view != null && view.IsMine)
        {
            CameraFollow cameraFollow = FindFirstObjectByType<CameraFollow>();
            if (cameraFollow != null) cameraFollow.target = playerInstance.transform;
        }
    }
}