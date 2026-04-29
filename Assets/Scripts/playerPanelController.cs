using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class playerPanelController : MonoBehaviourPunCallbacks
{
    public TMP_Text maxOyuncu;
    public TMP_Text oyuncuSayi;
    public TMP_Text odaAdi;
    public TMP_Text kurucuAd;
    public TMP_Text oyuncuAd;
    public GameObject startButton;

    void Start()
    {
        // Start'ta bilgileri çağırma → Photon verileri daha gelmemiş olur.
        // bilgiler();
        startButton.SetActive(false);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Update()
    {
        bilgiler();
        // Oyuncu sayısı sürekli güncellensin
        oyuncuSayi.text = PhotonNetwork.PlayerList.Length.ToString();
        startButtonController();
    }

    void bilgiler()
    {
        if (PhotonNetwork.InRoom)
        {
            maxOyuncu.text = PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
            oyuncuSayi.text = PhotonNetwork.PlayerList.Length.ToString();
            odaAdi.text = "Odanın Adı: " + PhotonNetwork.CurrentRoom.Name;
            kurucuAd.text = "Oda kurucusu: " + PhotonNetwork.CurrentRoom.CustomProperties["Kurucu"];
            oyuncuAd.text = "Senin adın: " + PhotonNetwork.NickName;
        }
        
    }

    public override void OnJoinedRoom()
    {
        bilgiler();
        startButtonController();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        bilgiler();
        startButtonController();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        bilgiler();
        startButtonController();
    }
    void startButtonController()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(false);
            return;
        }

        if(PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }
    public void oyunBaslat()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel("Oyun");
    }
}