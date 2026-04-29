using Photon.Pun;
using UnityEngine;
using System.Collections;

public class KarakterOlus : MonoBehaviourPunCallbacks
{
    public MapController mapController;

    private string[] karakterPrefab =
    {
        "Karakter_Gri",
        "Karakter_Mavi",
        "Karakter_Sari",
        "Karakter_Pembe",
        "Karakter_Mor",
        "Karakter_Yesil",
        "Karakter_Turuncu"
    };
    private void Start()
    {
        // Eğer sahneye geldiğimizde zaten odadaysak
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Odaya girildi, map başlatılıyor");
            mapController.InitMap();

            Debug.Log("Zaten odadayım, karakter oluşturuluyor (Start).");
            StartCoroutine(SpawWhenMapReady());

        }
    }
   
    public override void OnJoinedRoom()
    {
        Debug.Log("Odaya girildi, map başlatılıyor");
        mapController.InitMap();

        Debug.Log("Odaya yeni girildi, karakter oluşturuluyor (OnJoinedRoom).");
        StartCoroutine(SpawWhenMapReady());
    }
    void OnApplicationQuit()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }
    private void KarakteriOlustur()
    {
        SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
        Vector2 pos = spawnManager.getRandomPosition();

        int index;

        // EĞER daha önceden (Lobide vs.) bir karakter seçilmemişse
        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("KarakterIndex"))
        {
            // Püf Noktası Burası: Oyuncunun benzersiz ActorNumber'ını kullanıyoruz.
            // Böylece 1. oyuncu 0, 2. oyuncu 1 numaralı karakteri alır. Çakışma imkansız hale gelir.
            index = (PhotonNetwork.LocalPlayer.ActorNumber - 1) % karakterPrefab.Length;

            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
            props["KarakterIndex"] = index;
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
        else
        {
            // Eğer zaten bir seçim varsa onu bozma, oradan devam et
            index = (int)PhotonNetwork.LocalPlayer.CustomProperties["KarakterIndex"];
        }

        // Prefab ismini al ve oluştur
        string secilenPrefab = karakterPrefab[index];
        PhotonNetwork.Instantiate(secilenPrefab, pos, Quaternion.identity);

        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " için " + secilenPrefab + " oluşturuldu.");
    }
    private IEnumerator SpawWhenMapReady()
    {
        while (!mapController.mapReady)
        {
            yield return null;
        }
        KarakteriOlustur();
    }
}
