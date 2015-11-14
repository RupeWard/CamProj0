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
		DestroyStoppedSnap( );
	}

	public void Snap()
	{
		if (webCamTexture_.isPlaying)
		{
			Texture2D snap = CaptureImage( );
			if (snap == null)
			{
				LogManager.Instance.AddLine( "Can't snap when not playing" );
			}
			else
			{
				LogManager.Instance.AddLine( "Adding live image to Album" );
				AlbumTexture at = new AlbumTexture( );
				at.texture = snap;

				at = AlbumManager.Instance.AddToCurrentAlbum( at );

				//			byte[] bytes = snap.EncodeToPNG( );
				//			File.WriteAllBytes( fn, bytes );
			}
		}
		else
		{
			if (stoppedSnap_ != null)
			{
				LogManager.Instance.AddLine( "Adding paused image to Album" );
				AlbumTexture at = new AlbumTexture( );
				at.texture = stoppedSnap_;
				at = AlbumManager.Instance.AddToCurrentAlbum( at );
				stoppedSnap_ = null;
			}
			else
			{
				LogManager.Instance.AddLine( "No paused image to save" );
			}
		}
	}

	Texture2D stoppedSnap_;

	Texture2D CaptureImage()
	{
		Texture2D result = null;
		if (!webCamTexture_.isPlaying)
		{
			Debug.LogWarning( "Can't capture when not playing" );
		}
		else
		{
			{
				result = new Texture2D( webCamTexture_.width, webCamTexture_.height );
				result.SetPixels( webCamTexture_.GetPixels( ) );
				result.Apply( );
			}

		}

		return result;
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
		DestroyStoppedSnap( );
		webCamTexture_.Play();

    }

    void PauseCamera()
    {
        string msg = "DCD: " + (count++) + " PAUSE\n\n";
		DestroyStoppedSnap( );
		stoppedSnap_ = CaptureImage( );
		webCamTexture_.Pause();
        if (DEBUG_LOCAL)
        {
            Debug.Log(msg);
        }
		LogManager.Instance.AddLine(msg);
    }

	private void DestroyStoppedSnap()
	{
		if (stoppedSnap_ != null)
		{
			DestroyObject( stoppedSnap_ );
			stoppedSnap_ = null;
		}
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
		DestroyStoppedSnap( );
    }

}
