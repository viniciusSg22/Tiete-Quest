using Photon.Pun;
using UnityEngine.SceneManagement;

public class PhotonConnector : MonoBehaviourPunCallbacks
{
    void Start()
    {
        if (!PhotonNetwork.IsConnected) PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void DisconnectFromPhoton()
    {
        if (PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();
    }
}
