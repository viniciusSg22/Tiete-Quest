using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class PauseUI : MonoBehaviourPunCallbacks
{
    public GameObject PausePanel;

    public void Pause()
    {
        PausePanel.SetActive(true);
    }

    public void Continue()
    {
        PausePanel.SetActive(false);
    }

    public void Exit()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (!player.IsLocal) PhotonNetwork.CloseConnection(player);
            }
        }

        StartCoroutine(LeaveAndDisconnect());
    }

    private IEnumerator LeaveAndDisconnect()
    {
        PhotonNetwork.LeaveRoom();

        while (PhotonNetwork.InRoom)
        {
            yield return null;
        }

        PhotonNetwork.Disconnect();

        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }

        if (AudioManager.Instance != null) AudioManager.Instance.StopMusic();

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
