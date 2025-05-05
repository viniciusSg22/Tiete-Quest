using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public Transform[] spawnPoints;

    private void Start()
    {
        SpawnPlayerCharacter();
    }

    private void SpawnPlayerCharacter()
    {
        string characterImage = (string)PhotonNetwork.LocalPlayer.CustomProperties["CharacterImage"];

        GameObject characterPrefab = Resources.Load<GameObject>("Characters/" + characterImage);

        if (characterPrefab == null)
        {
            Debug.LogError("Personagem não encontrado em Resources/Characters/" + characterImage);
            return;
        }

        int index = System.Array.IndexOf(PhotonNetwork.PlayerList, PhotonNetwork.LocalPlayer);
        Transform spawnPoint = spawnPoints[index % spawnPoints.Length];

        PhotonNetwork.Instantiate("Characters/" + characterImage, spawnPoint.position, spawnPoint.rotation);
    }
}