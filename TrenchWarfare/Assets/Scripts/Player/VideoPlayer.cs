using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayer : MonoBehaviour
{
    public GameObject mainCamera;



    void Start()
    {
        var videoPlayer = this.AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.targetCamera = Camera.main;
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        videoPlayer.targetCameraAlpha = 0.005f;
        videoPlayer.isLooping = true;
        videoPlayer.url = "Assets/Videos/16MM Camera Overlay.mp4";
        

    }



}
