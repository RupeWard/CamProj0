using UnityEngine;
using System.Collections;

abstract public class WinControlPanel < TControlleeType >: MonoBehaviour 
{
	#region inspector hooks

	#endregion inspector hooks

	#region private data

	WinLayerWin win_;
	WinWin<TControlleeType> winWin_;

	TControlleeType controllee_;

	#endregion private data

	#region SetUp

	public void Init( WinLayerWin wlw, WinWin<TControlleeType> ww)
	{
		win_ = wlw;
		winWin_ = ww;

		gameObject.SetActive( true );
		controllee_ = wlw.GetComponent<TControlleeType>( );
		if (controllee_ == null)
		{
			controllee_ = wlw.transform.GetComponentInChildren<TControlleeType>( );
		}
		if (controllee_ == null)
		{
			Debug.LogError( "WCP: "+gameObject.name+" Failed to find controllee" );
		}
		else
		{
			PostInit( controllee_ );
		}

	}

public abstract void PostInit ( TControlleeType controllee);

	#endregion SetUp


	#region Button handlers


	public void OnBackButtonPressed()
	{
		win_.MoveToBack( );
	}

	public void OnCloseButtonPressed( )
	{
		transform.parent.GetComponent<WinControlsLayer>( ).CloseControls( );
	}

	public void OnMoveButtonPressed()
	{
		winWin_.MoveWindow( );
	}
	#endregion Button handlers


}
