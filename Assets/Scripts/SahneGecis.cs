using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SahneGecis : MonoBehaviour
{

    public void Giris()
    {
        SceneManager.LoadScene("OdaSecEkran");
    }
    public void odaKur()
    {
        GameData.basilanButonId = 1;
        SceneManager.LoadScene("Giris");
    }
    public void odaSec()
    {
        GameData.basilanButonId = 2;
        SceneManager.LoadScene("Giris");
    }
}
