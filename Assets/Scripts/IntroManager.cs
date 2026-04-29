using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    private VideoPlayer video;
    public string lastScaneName = "anaMenu";

    private void Awake()
    {
        video = GetComponent<VideoPlayer>();

        video.loopPointReached += SceneChange;
    }
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneChange(video);
        }
    }
    void SceneChange(VideoPlayer vp)
    {
        SceneManager.LoadScene(lastScaneName);
    }
    private void OnDestroy()
    {
        if (video != null)
            video.loopPointReached -= SceneChange;
    }
}
