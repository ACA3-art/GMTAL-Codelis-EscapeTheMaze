using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class KameraTakipSistemi : MonoBehaviour
{
    public Transform hedef;                 // Takip edilecek karakter
    public Vector3 offset = new Vector3(0, 0, -10);  // Kamera ile karakter arası
    public float hiz = 5f;                  // Takip hızı (Lerp ile yumuşak takip)

    void LateUpdate()
    {
        if (hedef == null) return; // Hedef yoksa çık

        Vector3 hedefPoz = hedef.position + offset;
        transform.position = Vector3.Lerp(transform.position, hedefPoz, hiz * Time.deltaTime);
    }
}
