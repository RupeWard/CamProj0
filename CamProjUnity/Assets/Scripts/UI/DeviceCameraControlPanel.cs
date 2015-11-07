using UnityEngine;
using System.Collections;

public class DeviceCameraControlPanel : MonoBehaviour
{
	#region inspector hooks

	public UnityEngine.UI.Text playButtonText;

	#endregion inspector hooks

	#region private data

	DeviceCameraDisplay deviceCameraDisplay_;

	#endregion private data

	#region SetUp

	public void Init( DeviceCameraDisplay dcd)
	{
		deviceCameraDisplay_ = dcd;
		SetPlayButtonText( );
		gameObject.SetActive( true );
	}

	private void SetPlayButtonText()
	{
		playButtonText.text = ((deviceCameraDisplay_.IsPlaying) ? ("STOP") : ("PLAY"));
	}
	#endregion SetUp


	#region Button handlers

	public void OnPlayButtonPressed( )
	{
		deviceCameraDisplay_.HandlePlayPause( );
		SetPlayButtonText( );
	}

	public void OnClearButtonPressed()
	{
		deviceCameraDisplay_.Clear( );
	}

	public void OnCloseButtonPressed( )
	{
		transform.parent.GetComponent<WinControlsLayer>( ).CloseControls( );
	}


	#endregion Button handlers


}
