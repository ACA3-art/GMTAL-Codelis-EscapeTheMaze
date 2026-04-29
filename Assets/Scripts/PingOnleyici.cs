using UnityEngine;
using Photon.Pun;

public class PingOnleyici : MonoBehaviourPun, IPunObservable
{
    public Vector3 rakipPos;
    [Range(1, 20)] public float gecikme = 8f;

    void Update()
    {
        // Eğer bu karakter bize ait değilse, pozisyonu yumuşat
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, rakipPos, gecikme * Time.deltaTime);
        }
    }

    // Bu fonksiyon PUN tarafından otomatik çağrılır (veri senkronizasyonu)
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Kendi pozisyonumuzu gönderiyoruz
            stream.SendNext(transform.position);
        }
        else
        {
            // Diğer oyuncudan pozisyon alıyoruz
            rakipPos = (Vector3)stream.ReceiveNext();
        }
    }
}
