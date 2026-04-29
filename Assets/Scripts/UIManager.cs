using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Finish UI")]
    public GameObject finishPanel;
    public RectTransform finishPanelRect;
    public TextMeshProUGUI rankText;

    [Header("Timer UI")]
    public GameObject timerObject;
    public TextMeshProUGUI timerText;

    [Header("Rank Animation")]
    public Vector2 centerPos = Vector2.zero;
    public Vector2 cornerPos = new Vector2(350, 200);
    public Vector3 smallScale = Vector3.one * 0.6f;

    [Header("Player Info")]
    public UnityEngine.UI.Image playerCharacterImage;
    public TextMeshProUGUI playerNameText;

    [Header("Character Sprites")]
    public Sprite[] characterSprite;

    private void Awake()
    {
        Instance = this;

        finishPanel.SetActive(false);
        timerObject.SetActive(false);
    }
    private void Start()
    {
        StartCoroutine(WaitForPlayerInfo());
    }
    IEnumerator WaitForPlayerInfo()
    {
        // Photon ağında her şeyin hazır olduğundan emin olana kadar bekle (maksimum 2 saniye)
        float timer = 0;
        while (!Photon.Pun.PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("KarakterIndex") && timer < 2f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        SetupPlayerInfo();
    }
    public void ShowRank(int rank)
    {
        finishPanel.SetActive(true);

        switch (rank)
        {
            case 1:
                rankText.text = "1. OLDUN! Tebrikler";
                rankText.color = new Color(1f, 0.84f, 0f); // altın sarısı
                break;

            case 2:
                rankText.text = "2. OLDUN!";
                rankText.color = new Color(0.75f, 0.75f, 0.75f); // gümüş
                break;

            case 3:
                rankText.text = "3. OLDUN!";
                rankText.color = new Color(0.8f, 0.5f, 0.2f); // bronz
                break;

            default:
                rankText.text = rank + ". oldun";
                rankText.color = Color.white;
                break;
        }

        StopAllCoroutines();
        StartCoroutine(RankAnimation());
    }

    IEnumerator RankAnimation()
    {
        RectTransform canvasRect = finishPanelRect.root.GetComponent<RectTransform>();

        // ekran ortası
        Vector2 center = Vector2.zero;

        // sağ üst köşe (canvas'a göre)
        Vector2 corner = new Vector2(
            canvasRect.rect.width / 2 - 190,
            canvasRect.rect.height / 2 - 90
        );

        finishPanelRect.anchoredPosition = center;
        finishPanelRect.localScale = Vector3.one;

        yield return new WaitForSeconds(1.5f);

        float t = 0;
        float duration = 1f;

        Vector2 startPos = finishPanelRect.anchoredPosition;
        Vector3 startScale = finishPanelRect.localScale;

        Vector3 targetScale = Vector3.one * 0.6f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;

            finishPanelRect.anchoredPosition =
                Vector2.Lerp(startPos, corner, lerp);

            finishPanelRect.localScale =
                Vector3.Lerp(startScale, targetScale, lerp);

            yield return null;
        }

        finishPanelRect.anchoredPosition = corner;
        finishPanelRect.localScale = targetScale;
    }

    public void ShowTimer()
    {
        timerObject.SetActive(true);
    }

    public void UpdateTimer(float time)
    {
        int kalanSure = Mathf.CeilToInt(time);
        timerText.text = kalanSure.ToString();

        // Renk değişimi
        if (kalanSure > 60)
        {
            timerText.color = Color.white;
        }
        else if (kalanSure > 30)
        {
            timerText.color = Color.yellow;
        }
        else if (kalanSure > 10)
        {
            timerText.color = new Color(1f, 0.5f, 0f); // Turuncu
        }
        else
        {
            timerText.color = Color.red;

            // Son 10 saniyede nabız efekti
            float pulse = 1f + Mathf.Abs(Mathf.Sin(Time.time * 8f)) * 0.4f;
            timerText.transform.localScale = Vector3.one * pulse;
        }

        // 10 saniyeden fazlaysa normal boyuta dön
        if (kalanSure > 10)
        {
            timerText.transform.localScale = Vector3.one;
        }
    }
    public void SetupPlayerInfo()
    {
        playerNameText.text = Photon.Pun.PhotonNetwork.NickName;

        if (Photon.Pun.PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("KarakterIndex"))
        {
            int index = (int)Photon.Pun.PhotonNetwork.LocalPlayer.CustomProperties["KarakterIndex"];

            if (index >= 0 && index < characterSprite.Length)
            {
                playerCharacterImage.sprite = characterSprite[index];
            }
        }
    }
}