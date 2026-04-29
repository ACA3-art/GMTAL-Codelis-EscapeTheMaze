using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KilitlemeSistemi : MonoBehaviour
{
    public TMP_InputField OdaKurPlayerName, OdaKurRoomName, KisiSayisi, OdaGirPlayerName,OdaGirRoomName;
    public Button OdaKurBtn, OdaGirBtn;
    public TMP_Text OdaKurText, OdaGirText;

    public void OdaKur()
    {
        OdaGirBtn.enabled = false;
        OdaGirPlayerName.enabled = false;
        OdaGirRoomName.enabled = false;
        OdaGirText.enabled = false;

        OdaGirPlayerName.text = null;
        OdaGirRoomName.text = null;
        
        OdaKurBtn.enabled = true;
        OdaKurPlayerName.enabled = true;
        OdaKurRoomName.enabled = true;
        OdaKurText.enabled = true;
        KisiSayisi.enabled = true;
    }
    public void OdaGir()
    {
        OdaGirBtn.enabled = true;
        OdaGirPlayerName.enabled = true;
        OdaGirRoomName.enabled = true;
        OdaGirText.enabled = true;

        OdaKurPlayerName.text = null;
        OdaKurRoomName.text = null;
        KisiSayisi.text = null;

        OdaKurBtn.enabled = false;
        OdaKurPlayerName.enabled = false;
        OdaKurRoomName.enabled = false;
        OdaKurText.enabled = false;
        KisiSayisi.enabled = false;
    }
    
}
