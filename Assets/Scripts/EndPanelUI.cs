using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class EndPanelUI : MonoBehaviour
{
    public static EndPanelUI Instance;

    [Header("Panel")]
    public GameObject endPanel;

    [Header("List")]
    public Transform playerListParent;
    public GameObject playerRowPrefab;

    [Header("Buttons")]
    public GameObject restartButton;
    public GameObject menuButton;

    private void Awake()
    {
        Instance = this;
        endPanel.SetActive(false);
    }

    public void OpenPanel()
    {
        endPanel.SetActive(true);
        restartButton.SetActive(PhotonNetwork.IsMasterClient);
        menuButton.SetActive(true);
    }

    public void AddPlayer(string playerName, string rank, int score)
    {
        GameObject row = Instantiate(playerRowPrefab, playerListParent);

        TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();

        texts[0].text = playerName;
        texts[1].text = rank;
        texts[2].text = score.ToString();

        Color rowColor = Color.white;

        switch (rank)
        {
            case "1.":
                rowColor = new Color(1f, 0.84f, 0f); // altın
                break;

            case "2.":
                rowColor = new Color(0.8f, 0.8f, 0.8f); // gümüş
                break;

            case "3.":
                rowColor = new Color(0.8f, 0.5f, 0.2f); // bronz
                break;

            case "DNF":
                rowColor = new Color(1f, 0.3f, 0.3f); // kırmızı
                break;
        }

        foreach (TMP_Text text in texts)
        {
            text.color = rowColor;
        }
    }
    public void RestartGame()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        PhotonNetwork.LocalPlayer.CustomProperties.Remove("KarakterIndex");
        PhotonNetwork.LoadLevel("OyunBekleme");
    }

    public void BackToMenu()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.Remove("KarakterIndex");
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("anaMenu");
    }
    public void ClearRows()
    {
        foreach (Transform child in playerListParent)
        {
            Destroy(child.gameObject);
        }
    }
}
