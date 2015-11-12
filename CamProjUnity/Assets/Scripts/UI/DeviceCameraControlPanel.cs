using UnityEngine;
using System.Collections;
using System;

public class DeviceCameraControlPanel : WinControlPanel <DeviceCameraDisplay>
{
	#region inspector hooks

	private ButtonSetButton playButton_;
	private ButtonSetButton clearButton_;

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

		playButton_ = CreateFuncButton( "Play", OnPlayButtonPressed );
		clearButton_ = CreateFuncButton( "Clear", OnClearButtonPressed );

		SetPlayButtonText( );
	}

	private void SetPlayButtonText()
	{
		playButton_.SetText( (deviceCameraDisplay_.IsPlaying) ? ("STOP") : ("PLAY"));
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

	public void OnSnapButtonPressed( )
	{
		deviceCameraDisplay_.Snap( );
	}

	#endregion Button handlers


}
