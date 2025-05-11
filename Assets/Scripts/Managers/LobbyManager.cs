using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Player Slots")]
    public Transform[] playerSlots;

    [Header("Ready Sprites")]
    public Sprite readySprite;
    public Sprite notReadySprite;

    [Header("Botões")]
    public Button readyButton;
    public Button cancelButton;

    [Header("Inputs")]
    public GameObject createInput;
    public GameObject joinInput;
    public TMP_Text readyButtonText;

    private void Start()
    {
        readyButton.onClick.AddListener(SetReady);
        cancelButton.onClick.AddListener(SetNotReady);
        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.InRoom) UpdateLobbyUI();
    }

    public void CreateRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            PhotonNetwork.CreateRoom(createInput.GetComponent<TMP_InputField>().text);
        }
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            PhotonNetwork.JoinRoom(joinInput.GetComponent<TMP_InputField>().text);
        }
    }

    public override void OnJoinedRoom()
    {
        string charName = PlayerPrefs.GetString("CharacterName", "Unknown");
        string charImage = PlayerPrefs.GetString("CharacterImage", "DefaultSprite");

        Hashtable props = new()
        {
            { "CharacterName", charName },
            { "CharacterImage", charImage },
            { "IsReady", false }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        UpdateLobbyUI();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateLobbyUI();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateLobbyUI();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        UpdateLobbyUI();
    }

    private void UpdateLobbyUI()
    {
        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < playerSlots.Length; i++)
        {
            GameObject slot = playerSlots[i].gameObject;

            if (i < players.Length)
            {
                Player player = players[i];

                string characterName = player.CustomProperties.ContainsKey("CharacterName") ? (string)player.CustomProperties["CharacterName"] : "Unknown";
                string imageName = player.CustomProperties.ContainsKey("CharacterImage") ? (string)player.CustomProperties["CharacterImage"] : "DefaultSprite";
                bool isReady = player.CustomProperties.ContainsKey("IsReady") && (bool)player.CustomProperties["IsReady"];

                var nameText = slot.transform.Find("SelectedCharacterName")?.GetComponent<TextMeshProUGUI>();
                if (nameText != null)
                    nameText.text = characterName;
                else
                    Debug.LogError($"[LobbyUI] 'SelectedCharacterName' não encontrado ou sem TextMeshProUGUI em {slot.name}");

                Image charImage = slot.transform.Find("SelectedCharacterImage").GetComponent<Image>();
                Sprite loadedSprite = Resources.Load<Sprite>("Art/Characters/" + imageName);
                if (loadedSprite != null) charImage.sprite = loadedSprite;

                Image statusImage = slot.transform.Find("LobbyStatus").GetComponent<Image>();
                statusImage.sprite = isReady ? readySprite : notReadySprite;

                slot.SetActive(true);
            }
            else
            {
                slot.SetActive(false);
            }
        }

        CheckAllPlayersReady();
    }

    private void SetReady()
    {
        if (PhotonNetwork.IsMasterClient && AllPlayersReady())
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("Sandbox");
        }
        else
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "IsReady", true } });
            CheckAllPlayersReady();
        }
    }

    private void SetNotReady()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "IsReady", false } });
        CheckAllPlayersReady();
    }

    private void CheckAllPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        bool allReady = true;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("IsReady") || !(bool)player.CustomProperties["IsReady"])
            {
                allReady = false;
                break;
            }
        }

        readyButtonText.text = allReady ? "Iniciar" : "Pronto";
    }

    private bool AllPlayersReady()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.ContainsKey("IsReady") || !(bool)player.CustomProperties["IsReady"])
            {
                return false;
            }
        }
        return true;
    }
}
