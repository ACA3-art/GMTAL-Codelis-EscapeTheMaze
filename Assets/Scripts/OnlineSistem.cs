using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class OnlineSistem : MonoBehaviourPunCallbacks
{
    [Header("UI References")]
    public TMP_InputField playerName1, roomName1, playerNumbers, playerName2, roomName2;

    private bool odaKurTiklandi = false;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    // Sunucuya bağlanıldığında lobiye otomatik giriş
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("Sunucuya bağlandı ve lobiye girildi.");
    }

    // Oda oluşturma
    public void OdaKur()
    {
        odaKurTiklandi = true;

        if (string.IsNullOrEmpty(playerName1.text) || string.IsNullOrEmpty(roomName1.text))
        {
            Debug.LogError("Oyuncu ismi veya oda ismi boş olamaz!");
            return;
        }

        if (!int.TryParse(playerNumbers.text, out int playerCount))
        {
            Debug.LogError("Oyuncu sayısı geçerli bir sayı olmalı!");
            return;
        }

        if (playerCount < 2 || playerCount > 5)
        {
            Debug.LogError("Oyuncu sayısı 2 ile 5 arası olmalıdır.");
            return;
        }

        PhotonNetwork.NickName = playerName1.text;

        RoomOptions odaAyar = new RoomOptions();
        odaAyar.MaxPlayers = (byte)playerCount;

        Hashtable customProps = new Hashtable();
        customProps["Kurucu"] = playerName1.text;
        odaAyar.CustomRoomProperties = customProps;
        odaAyar.CustomRoomPropertiesForLobby = new string[] { "Kurucu" };

        PhotonNetwork.CreateRoom(roomName1.text, odaAyar, TypedLobby.Default);
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void geriDon()
    {
        SceneManager.LoadScene("anaMenu");
    }

    // Odaya katılma
    public void OdaGir()
    {
        odaKurTiklandi = false;

        if (string.IsNullOrEmpty(playerName2.text) || string.IsNullOrEmpty(roomName2.text))
        {
            Debug.LogError("Oyuncu ismi veya oda ismi boş olamaz!");
            return;
        }

        PhotonNetwork.NickName = playerName2.text;
        PhotonNetwork.JoinRoom(roomName2.text);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Lobiye giriş başarılı.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Odaya giriş başarılı.");

        // Aynı isim kontrolü
        foreach (Player oyuncu in PhotonNetwork.PlayerList)
        {
            if (oyuncu != PhotonNetwork.LocalPlayer && oyuncu.NickName == PhotonNetwork.NickName)
            {
                Debug.LogWarning("Aynı isimle ikinci oyuncu bulunamaz.");
                PhotonNetwork.LeaveRoom();
                return;
            }
        }

        // Odaya başarıyla girildiyse sahne yüklenir
        PhotonNetwork.LoadLevel("OyunBekleme");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Odaya katılma başarısız: {message}");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Oda oluşturulamadı: {message}");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Odadan çıkıldı.");
    }
    void OnApplicationQuit()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }
}