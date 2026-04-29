using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSmanager : MonoBehaviour
{
    public static FPSmanager Instance;

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
        }
    }
    private void Start()
    {
        QualitySettings.vSyncCount = 0;

        ApplyFPS();
    }
    public void ApplyFPS()
    {
        int targetFPS = PlayerPrefs.GetInt("FPS", 60);

        Application.targetFrameRate = targetFPS;
    }
}
