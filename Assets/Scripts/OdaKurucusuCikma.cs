using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class OdaKurucusuCikma : MonoBehaviourPunCallbacks
{
    // Bir oyuncu odadan ayrıldığında otomatik tetiklenir
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        string playerName = otherPlayer.NickName;

        // Eğer odadayız ve "Kurucu" adında özel bir property varsa
        if (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Kurucu"))
        {
            string odaKurucusu = PhotonNetwork.CurrentRoom.CustomProperties["Kurucu"] as string;

            // Eğer çıkan oyuncu odanın kurucusuysa
            if (odaKurucusu == playerName)
            {
                Debug.Log($"Kurucu ({playerName}) oyundan çıktı, odadan ayrılıyoruz...");

                // Odayı terk et
                PhotonNetwork.LeaveRoom();

                // Giriş sahnesine dön
                PhotonNetwork.LoadLevel("Giris");
            }
            else
            {
                Debug.Log($"{playerName} odadan ayrıldı...");
            }
        }
    }
    void OnApplicationQuit()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }
}