using UnityEngine;
using System.Collections;
using System;

public class DeviceCameraControlPanel : WinControlPanel <DeviceCameraDisplay>
{
	#region inspector hooks

	public UnityEngine.UI.Text playButtonText;

	#endregion inspector hooks

	#region private data

	DeviceCameraDisplay deviceCameraDisplay_;

	#endregion private data

	public override string title( )
	{
		return "Camera";
	}

	#region SetUp

	public override void PostInit( DeviceCameraDisplay dcd)
	{
		if (dcd == null)
		{
			Debug.LogError( "NULL DCD" );
		}
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
