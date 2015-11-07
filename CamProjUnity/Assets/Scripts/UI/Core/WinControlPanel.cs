using UnityEngine;
using System.Collections;

abstract public class WinControlPanel : MonoBehaviour
{
	#region inspector hooks

	#endregion inspector hooks

	#region private data

	WinLayerWin win_;

	#endregion private data

	#region SetUp

	protected void Init( WinLayerWin wlw )
	{
		win_ = wlw;
		gameObject.SetActive( true );
	}

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

	#endregion Button handlers


}
