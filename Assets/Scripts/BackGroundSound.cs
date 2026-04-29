using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundSound : MonoBehaviour
{
    public static BackGroundSound Instance;

    public AudioSource source;
    public AudioClip clip;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        source = GetComponent<AudioSource>();
    }
    void Start()
    {
        source.clip = clip;
        source.loop = true;

        float savedVolume = PlayerPrefs.GetFloat("BackGroundSound", 0.75f);
        source.volume = savedVolume;

        source.Play();
    }


    void Update()
    {
        
    }
}
