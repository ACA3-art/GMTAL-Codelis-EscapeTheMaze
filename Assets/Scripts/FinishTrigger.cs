using UnityEngine;
using Photon.Pun;

public class FinishTrigger : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Finish:" + transform.position);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        PhotonView pv = other.GetComponentInParent<PhotonView>();

        if (pv != null && pv.CompareTag("Player") && pv.IsMine)
        {
            GameManager.Instance.photonView.RPC(
                "PlayerFinishedRPC",
                RpcTarget.MasterClient,
                PhotonNetwork.LocalPlayer.ActorNumber
            );
        }
    }

}