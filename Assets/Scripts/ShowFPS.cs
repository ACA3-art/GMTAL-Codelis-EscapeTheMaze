using UnityEngine;
using TMPro; // TextMeshPro kullanıyorsan bunu ekle

public class ShowFPS : MonoBehaviour
{
    public GameObject textObject; // Kapanıp açılacak olan Text objesi
    private float deltaTime = 0.0f;
    private TextMeshProUGUI fpsText;

    void Start()
    {
        fpsText = textObject.GetComponent<TextMeshProUGUI>();

        openShowFPS();
    }

    void Update()
    {
        openShowFPS();
        // Eğer obje kapalıysa hesaplama yapıp işlemciyi yorma
        if (!textObject.activeSelf) return;

        // FPS Hesaplama Mantığı
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = string.Format("{0:0.} FPS", fps);
    }

    void openShowFPS()
    {
        // Ayarlardan gelen değeri kontrol et (1: Açık, 0: Kapalı)
        bool isShow = PlayerPrefs.GetInt("ShowFPS", 1) == 1;

        // Eğer ayar kapalıysa objeyi tamamen kapat
        textObject.SetActive(isShow);
    }
}