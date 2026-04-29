using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SahneKarar : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject odaKur;
    public GameObject odaSec;
   
    void Start()
    {
        int oda = GameData.basilanButonId;
        if (oda == 1)
        {
            odaKur.SetActive(true);
        }
        else if (oda == 2)
        {
            odaSec.SetActive(true);
        }
        else
        {
            Debug.Log("secim yapılmadı");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
