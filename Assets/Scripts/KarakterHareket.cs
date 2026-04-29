using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class KarakterHareket : MonoBehaviourPunCallbacks
{
    [Header("Bileşenler")]
    public SpriteRenderer sr;
    public Rigidbody2D rb;
    //public TMP_Text isimText;

    [Header("Hareket Ayarları")]
    public float hiz = 6f;
    private Vector2 hareket;
    public Transform visual;
    private Vector3 visualStartScale;
    public FOVTest fov;
    [Header("CharacterSoundEffect")]
    public AudioSource source;
    public AudioClip clip;


    //odaBelirtmeUI ui;
    

    


    //public void ventGir()
    //{
    //    if (photonView.IsMine)
    //    {
    //        photonView.RPC("spriteKapat", RpcTarget.All);
    //    }
    //}
    
    private void Awake()
    {
        
    }
    private void Start()
    {
        source = GetComponent<AudioSource>();
        source.volume = PlayerPrefs.GetFloat("CharacterSound", 0.75f);

        Debug.Log("player" + transform.position);

        visualStartScale = visual.localScale;
        rb.freezeRotation = true;

        // Eğer bu karakter bana ait değilse kontrol devre dışı bırak
        if (!photonView.IsMine)
        {
            // Kamerayı kapat ve karakterden ayır
            Transform kameraObjesi = transform.Find("Kamera");
            if (kameraObjesi != null)
            {
                Camera cam = kameraObjesi.GetComponent<Camera>();
                if (cam != null) cam.enabled = false;
                kameraObjesi.SetParent(null);
            }

            //FOV Objesini Bul
            Transform digerFov = transform.Find("FOV");
            if (digerFov != null)
            {
                // Diğer oyuncunun görüş alanını siliyoruz ki bizim ekranımızı delmesin
                Destroy(digerFov.gameObject);
            }

            // İsim objesini karakterden ayır
            Transform isimObjesi = transform.Find("isim");
            if (isimObjesi != null)
            {
                isimObjesi.SetParent(null);
                TMP_Text text = isimObjesi.GetComponent<TMP_Text>();
            }

            // Hareketi kapat
            //enabled = false;

            // Oyuncunun ismini göster
            //if (isimText != null)
            //    isimText.text = photonView.Owner.NickName;
        }
        else
        {
        
            // Kamerayı aktif et ve hedefe bağla
            Transform kameraObjesi = transform.Find("Kamera");
            if (kameraObjesi != null)
            {
                Camera cam = kameraObjesi.GetComponent<Camera>();
                if (cam != null)
                {
                    kameraObjesi.SetParent(null);
                    cam.enabled = true;

                    KameraTakipSistemi takip = cam.GetComponent<KameraTakipSistemi>();
                    if (takip != null)
                        takip.hedef = transform;
                }
            }

            // 2. KENDİ FOV'UNU AKTİF ET VE AYIR
            Transform benimFov = transform.Find("FOV");
            if (benimFov != null)
            {
                benimFov.SetParent(null); // Karakterle birlikte dönmemesi için ayırıyoruz

                var fovScript = benimFov.GetComponent<FOVTest>();
                if (fovScript != null)
                {
                    fovScript.target = transform; // Takip etmesi için hedef veriyoruz
                }
            }
        }
        //ses efect ayar

        source.playOnAwake = false;
        source.loop = false;
    }

    private void Update()
    {
        // Sadece kendi karakterimse kontrol et
        
        if (!photonView.IsMine) return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (hareket.magnitude > 0) // Eğer karakter kımıldıyorsa
        {
            if (!source.isPlaying) // Ve şu an bir ses çalmıyorsa
            {
                source.volume = PlayerPrefs.GetFloat("CharacterSound", 0.75f);
                source.pitch = Random.Range(0.9f, 1.1f);
                source.clip = clip;
                source.loop = true; // Yürüme sesi döngüde olmalı
                source.Play();
            }
        }
        else // Karakter durduğunda
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }

        hareket = new Vector2(x, y).normalized;
        /*
        if (x > 0)
        {
            //transform.localScale = new Vector3(2, 2.5f, 1);
            photonView.RPC("AciDegistir", RpcTarget.AllBuffered, 1);
        }    
        else if (x < 0)
        {
            //transform.localScale = new Vector3(-2, 2.5f, 1);
            photonView.RPC("AciDegistir", RpcTarget.AllBuffered, -1); ;
        }*/
        if (x > 0)
        {
            
            photonView.RPC("AciDegistir", RpcTarget.AllBuffered, false);
        }
        else if (x < 0)
        {

            photonView.RPC("AciDegistir", RpcTarget.AllBuffered, true);
        }


    }
   
    [PunRPC]
    void AciDegistir(bool yon)
    {
        sr.flipX = yon;
    }
    
    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        rb.velocity = hareket * hiz;
    }
    //string[] odaTag = { "toplantiOdasi", "resimOdasi", "depoOdasi", "LaboratuvarOdasi", "büyüOdasi", "kutuphaneOdasi", "elektrikOdasi", "sandikOdasi", "yemekOdasi", "mahzenOdasi" };
    //string[] odaIsim = { "Toplantı Odası", "Resim Odası", "Depo", "Laboratuvar", "Büyü Odası", "Kütüphane", "Elektrik Odası", "Sandık Odası", "Yemekhane","Şarap Mahzeni" };
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //for (int i = 0; i < 10; i++)
        //{
        //    if (collision.gameObject.CompareTag(odaTag[i]))
        //    {
        //        ui.odaBelirtme.SetActive(true);
        //        ui.odaBelitmeText.text = odaIsim[i];
        //    }
        //}
        //if (collision.gameObject.CompareTag("teleport"))
        //{
        //    string rolum = PhotonNetwork.LocalPlayer.CustomProperties["Rol"] as string;
        //    if (rolum.ToLower() == "katil")
        //    {
        //        ui.VentObjesiKatil.SetActive(true);
        //    }
        //}

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //for (int i = 0; i < odaTag.Length; i++)
        //{
        //    if (collision.gameObject.CompareTag(odaTag[i]))
        //    {
        //        ui.odaBelirtme.SetActive(false);
        //    }
        //}
      
    }
}

        