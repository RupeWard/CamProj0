using UnityEngine;
using System.Collections;

public class WinWin < TWinType> : MonoBehaviour 
{
	private static readonly bool DEBUG_LOCAL = true;

	public GameObject controlPanelPrefab;

	private WinControlPanel<TWinType> controlPanel_ = null;

	public WinLayerWin winLayerWin;

	public void HandleLossOfFocus( )
	{
		if (controlPanel_ != null)
		{
			controlPanel_.OnCloseButtonPressed( );
		}
	}

	public void HandleClick( )
	{
		if (winLayerWin.MovetoTop( ))
		{
			if (DEBUG_LOCAL)
			{
				Debug.Log( "WW: "+gameObject.name+".HandleClick() moved to top" );
			}
		}
		else
		{
			if (controlPanel_ == null)
			{
				GameObject go = Instantiate( controlPanelPrefab ) as GameObject;
				controlPanel_ = go.GetComponent< WinControlPanel<TWinType>  >( );
			}
			if (controlPanel_ == null)
			{
				Debug.LogError( "WW: Failed to make control panel" );
			}
			else
			{
				winLayerWin.WinLayerManager.SetControls( controlPanel_.GetComponent<RectTransform>( ) );
				controlPanel_.Init( winLayerWin );
				if (DEBUG_LOCAL)
				{
					Debug.Log( "WW: HandleClik() opened controls" );
				}
			}
		}
	}

	public void MoveToBack( )
	{
		if (DEBUG_LOCAL)
		{
			Debug.Log( "WW: MoveToBack" );
		}
		if (winLayerWin.currentLayer.WinLayerManager.NumLayers < 2)
		{
			Debug.Log( "WW: No move as only one layer" );
		}
		else if (winLayerWin.currentLayer.LayerNum == 0)
		{
			Debug.Log( "WW: No move as already at back " + winLayerWin.currentLayer.DebugDescribe( ) );
		}
		else
		{
			if (winLayerWin.currentLayer.IsOnTop)
			{
				if (DEBUG_LOCAL)
				{
					Debug.Log( "WW: Calling HandleLossOfFocus as on top " + winLayerWin.currentLayer.DebugDescribe( ) );
				}
				HandleLossOfFocus( );
			}
			else
			{
				if (DEBUG_LOCAL)
				{
					Debug.Log( "WW: Not Calling HandleLossOfFocus " + winLayerWin.currentLayer.DebugDescribe( ) );
				}
			}
			winLayerWin.MoveToBack( );
		}
	}



}
