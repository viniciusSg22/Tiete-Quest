using Photon.Pun;

public class ExitPortal : MonoBehaviourPunCallbacks, IInteractable
{
    public string sceneToLoad;

    public bool CanInteract() => true;

    public void Interact()
    {
        photonView.RPC(nameof(RequestSceneChange), RpcTarget.MasterClient, sceneToLoad);
    }

    [PunRPC]
    void RequestSceneChange(string requestedScene)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (AudioManager.Instance != null) AudioManager.Instance.StopMusic();

        PhotonNetwork.LoadLevel(requestedScene);
    }
}
