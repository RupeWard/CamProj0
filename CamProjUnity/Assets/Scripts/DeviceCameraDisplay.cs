using UnityEngine;
using System.Collections;

public class DeviceCameraDisplay : MonoBehaviour
{
    private static readonly bool DEBUG_LOCAL = true;

    public UnityEngine.UI.RawImage rawImage;

    private WebCamTexture webCamTexture_ = null;
    private RectTransform myRectTransform_;

	void Start ()
    {
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
            Debug.Log(msg);
            LogPanel.Instance.Append(msg);
        }
        webCamTexture_.Play();

    }

    void PauseCamera()
    {
        if (DEBUG_LOCAL)
        {
            Debug.Log("DCD: PAUSE");
        }
        webCamTexture_.Pause();
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
}
