using Photon.Pun;

public class ExitPortal : MonoBehaviourPunCallbacks, IInteractable
{
    public string sceneToLoad;

    public bool CanInteract() => true;

    public void Interact()
    {
        if (PhotonNetwork.IsMasterClient) PhotonNetwork.LoadLevel(sceneToLoad);
        else photonView.RPC("RequestSceneChange", RpcTarget.MasterClient);
    }

    [PunRPC]
    void RequestSceneChange()
    {
        if (PhotonNetwork.IsMasterClient) PhotonNetwork.LoadLevel(sceneToLoad);
    }
}
