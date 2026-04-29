using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class MenuController : MonoBehaviourPunCallbacks
{
    public bool menutiklandi = false;
    public bool minimaptiklandi = false;
    public GameObject canvas;

    
    
    // Menü aç/kapa butonu
    public void Menu_ac_kapa()
    {
        menutiklandi = !menutiklandi;
        canvas.SetActive(menutiklandi);
    }

    // Odadan çıkma işlemi
    public void Cik()
    {
        Debug.Log("Odadan ayrılma komutu gönderiliyor...");
        PhotonNetwork.LeaveRoom();
    }

    // Oda başarıyla terk edildiğinde çağrılır (otomatik)
    public override void OnLeftRoom()
    {
        Debug.Log("Odadan başarıyla ayrıldım. Ana menüye ('Giris') yönlendiriliyorum.");
        SceneManager.LoadScene("Giris");
    }

    // Odaya bağlı bir oyuncu ayrıldığında çağrılır (otomatik)
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"{otherPlayer.NickName} odadan ayrıldı.");

        // Oda kurucusu ayrıldıysa herkes lobiden çıkar
        if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Kurucu"))
        {
            string odaKurucusu = (string)PhotonNetwork.CurrentRoom.CustomProperties["Kurucu"];

            if (odaKurucusu == otherPlayer.NickName)
            {
                Debug.Log("Oda kurucusu ayrıldı! Oda kapatılıyor, ana menüye dönülüyor.");
                PhotonNetwork.LeaveRoom();
            }
        }
    }
}

