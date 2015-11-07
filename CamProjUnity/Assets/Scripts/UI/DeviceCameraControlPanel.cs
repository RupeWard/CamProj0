using UnityEngine;
using System.Collections;

public class DeviceCameraControlPanel : WinControlPanel
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
		Init( dcd.winLayerWin );
		deviceCameraDisplay_ = dcd;
		SetPlayButtonText( );
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

	#endregion Button handlers


}
