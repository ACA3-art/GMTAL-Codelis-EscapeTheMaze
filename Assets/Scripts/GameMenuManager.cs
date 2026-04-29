using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    [Header("Kapanacak Objeler")]
    public GameObject[] closedObjects;

    [Header("Ayarlar Menüsü")]
    public GameObject settingPanel;

    [Header("Ses Ayarları")]
    public Slider backgroundSlider;
    public Slider characterSlider;

    [Header("FPSButton")]
    public Button[] fpsButton;
    public Toggle ShowFPS;

    public Color activeColor = Color.green;
    public Color passiveColor = Color.white;

    private void Start()
    {
        if (backgroundSlider != null) backgroundSlider.value = PlayerPrefs.GetFloat("BackGroundSound", 0.75f);
        if (characterSlider != null) characterSlider.value = PlayerPrefs.GetFloat("CharacterSound", 0.75f);
        if (ShowFPS != null) ShowFPS.isOn = PlayerPrefs.GetInt("ShowFPS", 1) == 1;

        int nowFPS = PlayerPrefs.GetInt("FPS", 60);
        UpdateButtonColors(nowFPS);
    }
    public void OpenSettingPanel()
    {
        for(int i = 0; i< closedObjects.Length; i++)
        {
            if (closedObjects[i] != null) closedObjects[i].SetActive(false);
        }
        if (settingPanel != null) settingPanel.SetActive(true);
    }
    public void CloseSettingPanel()
    {
        for(int i = 0; i < closedObjects.Length; i++)
        {
            if (closedObjects[i] != null) closedObjects[i].SetActive(true);
        }
        if (settingPanel != null) settingPanel.SetActive(false);
    }

    public void SetBackGroundSound(float soundValue)
    {
        PlayerPrefs.SetFloat("BackGroundSound", soundValue);
        PlayerPrefs.Save();

        BackGroundSound.Instance.source.volume = soundValue;
    }
    public void SetCharacterSound(float soundValue)
    {
        PlayerPrefs.SetFloat("CharacterSound", soundValue);
        PlayerPrefs.Save();
    }

    public void SetFPSValue(int value)
    {
        PlayerPrefs.SetInt("FPS", value);
        PlayerPrefs.Save();

        if (FPSmanager.Instance != null)
        {
            FPSmanager.Instance.ApplyFPS();
        }

        UpdateButtonColors(value);
    }
    private void UpdateButtonColors(int currentFPS)
    {
        foreach (Button b in fpsButton)
        {
            if (b != null) b.image.color = passiveColor;
        }

        if (currentFPS == 30 && fpsButton.Length > 0) fpsButton[0].image.color = activeColor;
        else if (currentFPS == 45 && fpsButton.Length > 1) fpsButton[1].image.color = activeColor;
        else if (currentFPS == 60 && fpsButton.Length > 2) fpsButton[2].image.color = activeColor;
        else if (currentFPS == 90 && fpsButton.Length > 3) fpsButton[3].image.color = activeColor;
        else if (currentFPS == 120 && fpsButton.Length > 4) fpsButton[4].image.color = activeColor;
        else if (currentFPS == -1 && fpsButton.Length > 5) fpsButton[5].image.color = activeColor;
    }
    public void SetFPSOpen(bool isOpen)
    {
        PlayerPrefs.SetInt("ShowFPS", isOpen ? 1 : 0);
        PlayerPrefs.Save();
    }
}
