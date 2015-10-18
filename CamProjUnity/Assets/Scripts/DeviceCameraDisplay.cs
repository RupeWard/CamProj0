﻿using UnityEngine;
using System.Collections;

public class DeviceCameraDisplay : MonoBehaviour
{
    private static readonly bool DEBUG_LOCAL = true;

    public UnityEngine.UI.RawImage rawImage;

    private WebCamTexture webCamTexture_ = null;
    private RectTransform myRectTransform_;

	void Start ()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("DCD Start: " + devices.Length + " devices");
        for (int i= 0; i < devices.Length; i++)
        {
            sb.Append("\n "+i+" "+devices[i].name+" "+devices[i].isFrontFacing);
        }
        webCamTexture_ = new WebCamTexture();
        myRectTransform_ = GetComponent<RectTransform>();        
	}
	
	void Update ()
    {
        if (rawImage != null)
        {
            rawImage.texture = webCamTexture_;
        }
    }

    int count = 0;

    void PlayCamera()
    {
        if (DEBUG_LOCAL)
        {
            string msg = "DCD: "+(count++)+" PLAY size = " + webCamTexture_.width + " * " + webCamTexture_.height
                +"\nScreen = "+Screen.width+" x "+Screen.height
                +"\nOrientation = "+Input.deviceOrientation
                +"\n";
            if (DEBUG_LOCAL)
            {
                Debug.Log(msg);
            }
            LogPanel.Instance.Append(msg);
        }
        webCamTexture_.Play();

    }

    void PauseCamera()
    {
        string msg = "DCD: " + (count++) + " PAUSE\n\n";
        webCamTexture_.Pause();
        if (DEBUG_LOCAL)
        {
            Debug.Log(msg);
        }
        LogPanel.Instance.Append(msg);
    }

    public void HandlePlayPause()
    {
        if (webCamTexture_.isPlaying)
        {
            PauseCamera();
        }
        else
        {
            PlayCamera();
        }
    }

    void OnDestroy()
    {
        if (webCamTexture_ != null && webCamTexture_.isPlaying)
        {
            webCamTexture_.Stop();
        }
    }
}
