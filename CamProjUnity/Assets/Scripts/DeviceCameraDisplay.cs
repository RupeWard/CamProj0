using UnityEngine;
using System.Collections;

public class DeviceCameraDisplay : WinWin< DeviceCameraDisplay>
{
    private static readonly bool DEBUG_LOCAL = true;

    public UnityEngine.UI.RawImage rawImage;
	public Texture2D clearTexture;

    private WebCamTexture webCamTexture_ = null;
    private RectTransform myRectTransform_;

	protected override void Start ()
    {
		base.Start( );

        WebCamDevice[] devices = WebCamTexture.devices;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("DCD Start: " + devices.Length + " devices");
        for (int i= 0; i < devices.Length; i++)
        {
            sb.Append("\n "+i+" "+devices[i].name+" "+devices[i].isFrontFacing);
        }
		if (DEBUG_LOCAL)
		{
			Debug.Log( sb.ToString( ) );
		}
        webCamTexture_ = new WebCamTexture();
        myRectTransform_ = GetComponent<RectTransform>();
		PlayCamera( );       
	}
	
	void Update ()
    {
        if (rawImage != null)
        {
            if (webCamTexture_.isPlaying)
            {
                rawImage.texture = webCamTexture_;
            }
        }
    }

	public bool IsPlaying
	{
		get { return (webCamTexture_ != null && webCamTexture_.isPlaying);  }
	}

	public void Clear( )
	{
		if (!IsPlaying)
		{
			rawImage.texture = clearTexture;
		}
	}

	public void Snap()
	{
		if (!webCamTexture_.isPlaying)
		{
			Debug.LogWarning( "Can't snap when not playing" );
			LogManager.Instance.AddLine( "Can't snap when not playing" );
		}
		else
		{
			Texture2D snap = null;
			
			{
				snap = new Texture2D( webCamTexture_.width, webCamTexture_.height );
				snap.SetPixels( webCamTexture_.GetPixels( ) );
				snap.Apply( );
			}
			
			AlbumTexture at = new AlbumTexture( );
			at.texture = snap;

			at = AlbumManager.Instance.AddToCurrentAlbum( at );

			//			byte[] bytes = snap.EncodeToPNG( );
			//			File.WriteAllBytes( fn, bytes );
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
            LogManager.Instance.AddLine(msg);
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
        LogManager.Instance.AddLine(msg);
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
