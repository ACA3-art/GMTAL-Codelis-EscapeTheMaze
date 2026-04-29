using UnityEngine;
using Photon.Pun;

public class MapController : MonoBehaviourPun
{
    int[] angles = { 0, 90, 180, 270 };
    public bool mapReady = false;
    public void InitMap()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        int angle = angles[Random.Range(0, angles.Length)];
        bool flipX = Random.value > 0.5f;

        photonView.RPC("SetMapTransform", RpcTarget.AllBuffered, angle, flipX);
    }

    [PunRPC]
    void SetMapTransform(int angle, bool flipX)
    {
        Debug.Log("Map güncellendi -> angle: " + angle + " flip: " + flipX);

        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (flipX)
            transform.localScale = new Vector3(-1, 1, 1);

        mapReady = true;
    }
}