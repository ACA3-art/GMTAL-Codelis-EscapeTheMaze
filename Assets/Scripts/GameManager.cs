using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    private int finishCount = 0;
    private bool timerStarted = false;
    private float timer = 0f;
    public float maxTime = 120f;
    private bool gameEnded = false;

    private Dictionary<int, int> ranks = new Dictionary<int, int>();

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (timerStarted)
        {
            timer -= Time.deltaTime;

            UIManager.Instance.UpdateTimer(timer);

            if(timer <= 0)
            {
                timerStarted = false;
                EndGame();
            }
        }
    }
    [PunRPC]
    void PlayerFinishedRPC(int actorNumber)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        // Aynı oyuncu ikinci kez sayılmasın
        if (ranks.ContainsKey(actorNumber)) return;

        finishCount++;
        ranks[actorNumber] = finishCount;

        photonView.RPC("UpdateRank", RpcTarget.All, actorNumber, finishCount);
        photonView.RPC("ShowPlayerRank", RpcTarget.All, actorNumber, finishCount);

        CheckStartTimer();

        // Herkes bitirdiyse süreyi bekleme
        if (ranks.Count >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            timerStarted = false;
            EndGame();
        }
    }
    void CheckStartTimer()
    {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        int required = Mathf.CeilToInt(playerCount * 0.5f);

        if (!timerStarted && finishCount >= required)
        {
            photonView.RPC("StartTimer", RpcTarget.All);
        }
    }

    void EndGame()
    {
        // KRİTİK: Sadece Master Client sonuç listesini hesaplayabilir 
        // çünkü 'ranks' verisi sadece onda var.
        if (!PhotonNetwork.IsMasterClient) return;

        if (gameEnded) return;
        gameEnded = true;

        List<PlayerResult> results = new List<PlayerResult>();

        foreach (var p in PhotonNetwork.PlayerList)
        {
            int rank;
            // Ranks sözlüğü sadece Master'da dolu olduğu için burada doğru çalışır.
            if (!ranks.TryGetValue(p.ActorNumber, out rank))
            {
                rank = 999;
            }

            int score = GetScore(rank);

            results.Add(new PlayerResult
            {
                name = p.NickName,
                rank = rank,
                score = score
            });
        }

        // Sıralama
        results.Sort((a, b) => a.rank.CompareTo(b.rank));

        PlayerResultList wrapper = new PlayerResultList();
        wrapper.players = results.ToArray();

        string json = JsonUtility.ToJson(wrapper);

        // Şimdi hazırlanan bu doğru listeyi herkese gönderiyoruz.
        photonView.RPC("ShowEndPanel", RpcTarget.All, json);
    }

    [PunRPC]
    void ShowEndPanel(string json)
    {
        EndPanelUI.Instance.ClearRows();

        PlayerResultList resultList = JsonUtility.FromJson<PlayerResultList>(json);

        EndPanelUI.Instance.OpenPanel();

        foreach (var p in resultList.players)
        {
            string rankText = p.rank == 999 ? "DNF" : p.rank + ".";

            EndPanelUI.Instance.AddPlayer(
                p.name,
                rankText,
                p.score
            );
        }
    }

    int GetScore(int rank)
    {
        switch (rank)
        {
            case 1: return 500;
            case 2: return 400;
            case 3: return 300;
            case 4: return 200;
            case 5: return 100;
            default: return 50; // DNF
        }
    }
    [System.Serializable]
    public class PlayerResult
    {
        public string name;
        public int rank;
        public int score;
    }

    [System.Serializable]
    public class PlayerResultList
    {
        public PlayerResult[] players;
    }

    [PunRPC]
    void UpdateRank(int actorNumber, int rank)
    {
        Debug.Log("Player " + actorNumber + " rank: " + rank);
    }

    [PunRPC]
    void GiveScore(int actorNumber, int score)
    {
        Debug.Log("Player " + actorNumber + " score: " + score);
    }
    
    [PunRPC]
    void ShowPlayerRank(int actorNumber, int rank)
    {
        if (Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            UIManager.Instance.ShowRank(rank);
        }
    }
    [PunRPC]
    void StartTimer()
    {
        timerStarted = true;
        timer = maxTime;

        UIManager.Instance.ShowTimer();
    }
}
