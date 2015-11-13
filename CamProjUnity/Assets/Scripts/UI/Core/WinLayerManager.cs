﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(RectTransform))]
public class WinLayerManager : SingletonSceneLifetime< WinLayerManager >, IDebugDescribable
{
	private static readonly bool DEBUG_LOCAL = false;

    #region Interface

	public void DebugDescribe( System.Text.StringBuilder sb)
	{
		sb.Append( "[WLM: " ).Append( NumLayers ).Append( " layers" );
		for (int i =0; i < winLayerDefns_.Count; i++)
		{
			sb.Append( "\n" ).Append( i ).Append( " " );
			if (winLayerDefns_[i]==null)
			{
				sb.Append( "null" );
			}
			else
			{
				winLayerDefns_[i].DebugDescribe( sb );
			}
		}
	}

    public int NumLayers
    {
		get { return winLayerDefns_.Count;  }
	}

	public WinLayerWin InstantiateToLayer( GameObject prefab )
	{
		GameObject go = Instantiate( prefab ) as GameObject;
		WinLayerWin win = go.GetComponent<WinLayerWin>( );
		Add( win );
		return win;
	}

	public WinLayerWin InstantiateToTopLayer( GameObject prefab )
	{
		GameObject go = Instantiate( prefab ) as GameObject;
		WinLayerWin win = go.GetComponent<WinLayerWin>( );
		AddToTopLayer( win );
		return win;
	}

	public WinLayerWin InstantiateToOverlaysLayer( GameObject prefab )
	{
		GameObject go = Instantiate( prefab ) as GameObject;
		WinLayerWin win = go.GetComponent<WinLayerWin>( );
		AddToOverlaysLayer( win );
		currentOverlay_ = win.gameObject;
		return win;
	}

	public GameObject confirmPanelPrefab;
	private GameObject currentOverlay_;

	public void CreateConfirmPanel(ConfirmPanel.Data data)
	{ 
		if (currentOverlay_== null)
		{
			GameObject go = Instantiate( confirmPanelPrefab) as GameObject;
			ConfirmPanel confirmPanel= go.GetComponent<ConfirmPanel>( );
			if (confirmPanel == null)
			{
				Debug.LogError( "WLM: Failed to make confirm panel" );
			}
			else
			{
				if (confirmPanel.GetComponent<RectTransform>() ==null)
				{
					Debug.LogError( "NULL RT" );
				}
				SetOverlays( confirmPanel.GetComponent<RectTransform>( ) );
				confirmPanel.Init( data );
				currentOverlay_ = confirmPanel.gameObject;
				if (DEBUG_LOCAL)
                {
					Debug.Log( "WLM: created confirm panel" );
				}		
			}
		}
		else
		{
			Debug.LogError( "Can't create confirm panel while another overay is present they all block" );
		}
	}

	private void SetOverlays(RectTransform r)
	{
		winOverlaysLayer.SetOverlays( r );
	}

	public void CloseOverlays()
	{
		winOverlaysLayer.CloseOverlays( );
	}

	public void SetControls( RectTransform r )
		{
			winControlsLayer.SetControls( r );
		}

	public void CloseControls( )
	{
		winControlsLayer.CloseControls(  );
	}
	

	public bool RemoveContentsFromLayer(WinLayerWin wlw)
	{
		bool bRemoved = false;
		WinLayerDefn toRemove = null;
		WinLayerWin removedWin = null;
		for ( int i = 0; i < winLayerDefns_.Count; i++)
		{
			if (!bRemoved)
			{
				if (winLayerDefns_[i].ContainsContents( wlw ))
				{
					removedWin = winLayerDefns_[i].ReleaseContents( );
					toRemove = winLayerDefns_[i];
					bRemoved = true;
				}
			}
			else
			{
				winLayerDefns_[i].LayerNum = (i - 1);
			}
		}
		if (toRemove != null)
		{
			if (removedWin.lossOfFocusAction != null)
			{
				removedWin.lossOfFocusAction( );
			}
			GameObject.Destroy( toRemove.gameObject );
			winLayerDefns_.Remove( toRemove );
		}
		if (!bRemoved)
		{
			Debug.LogWarning( "Failed to remove WLW" );
		}
		return bRemoved;
	}

	public bool MoveContentsToTop( int layerNum )
	{
		bool bMoved = false;
		if (layerNum != (NumLayers-1))
		{
			if (DEBUG_LOCAL)
			{
				Debug.Log( "WLM: moving contents of layer " + layerNum + " to top " + winLayerDefns_[layerNum].DebugDescribe( ) );
//				Debug.Log(this.DebugDescribe( ));
			}

			WinLayerWin newTopContents = winLayerDefns_[layerNum].ReleaseContents( );
			newTopContents.currentLayer = null;

			if (DEBUG_LOCAL)
			{
//				Debug.Log( "After " + layerNum + " removed" );
//				Debug.Log( this.DebugDescribe( ) );
			}

			for (int i = layerNum; i < (NumLayers - 1); i++)
			{
				WinLayerWin content = winLayerDefns_[i + 1].ReleaseContents( );
				if (content != null)
				{
					content.AddToWinLayer( winLayerDefns_[i] );
					if (DEBUG_LOCAL)
					{
						Debug.Log( content.gameObject.name + " moved to layer " + winLayerDefns_[i].DebugDescribe( ) );
//						Debug.Log( this.DebugDescribe( ) );
					}
				}
				else
				{
					Debug.LogWarning( "WLM: empty layer "+winLayerDefns_[layerNum+1].DebugDescribe() );
				}

			}
			if (DEBUG_LOCAL)
			{
//				Debug.Log( "After moved" );
//				Debug.Log( this.DebugDescribe( ) );
			}
			if (newTopContents)
			{
				newTopContents.AddToWinLayer( winLayerDefns_[NumLayers - 1] );
				if (DEBUG_LOCAL)
				{
					Debug.Log( newTopContents.gameObject.name + " moved to top layer " + winLayerDefns_[NumLayers-1].DebugDescribe( ) );
//					Debug.Log( this.DebugDescribe( ) );
				}
			}
			else
			{
				Debug.LogWarning( "WLM: no newTopContents" );
			}
			bMoved = true;
		}
		else
		{
			if (DEBUG_LOCAL)
			{
				Debug.Log( "WLM: layer " + layerNum + " is on top " );
			}
		}
		return bMoved;
	}

	public bool MoveContentsToBack( int layerNum )
	{
		bool bMoved = false;
		if (layerNum != 0)
		{
			if (DEBUG_LOCAL)
			{
				Debug.Log( "WLM: moving contents of layer " + layerNum + " to back" );
			}

			WinLayerWin newBackContents = winLayerDefns_[layerNum].ReleaseContents( );
			newBackContents.currentLayer = null;
			for (int i = layerNum; i >0; i--)
			{
				WinLayerWin content = winLayerDefns_[i-1].ReleaseContents( );
				if (content != null)
				{
					content.AddToWinLayer( winLayerDefns_[i] );
					if (DEBUG_LOCAL)
					{
						Debug.Log( content.gameObject.name + " moved to layer " + winLayerDefns_[layerNum].DebugDescribe( ) );
					}
				}
				else
				{
					Debug.LogWarning( "WLM: empty layer " + winLayerDefns_[i].DebugDescribe( ) );
				}

			}
			if (newBackContents)
			{
				newBackContents.AddToWinLayer( winLayerDefns_[0] );
				if (DEBUG_LOCAL)
				{
					Debug.Log( newBackContents.gameObject.name + " moved to backlayer " + winLayerDefns_[0].DebugDescribe( ) );
				}
			}
			else
			{
				Debug.LogWarning( "WLM: no newBackContents" );
			}
			bMoved = true;
		}
		else
		{
			if (DEBUG_LOCAL)
			{
				Debug.Log( "WLM: layer " + layerNum + " is at back" );
			}
		}
		return bMoved;
	}

	#endregion Interface

	#region settings

	#region settings

	public int numStartingLayers = 0;

    #endregion inspector hooks

    public GameObject winLayerPrefab;
	public RectTransform winLayersContainer;
	public RectTransform topLayerContainer;
	public RectTransform controlsLayerContainer;
	public RectTransform overlaysLayerContainer;

	public WinControlsLayer winControlsLayer;
	public WinOverlaysLayer winOverlaysLayer;

	#endregion inspector hooks

	#region cached hooks

	private RectTransform rectTransform_;
    public RectTransform RectTransform
    {
        get { return rectTransform_; }
    }

    #endregion cached hooks

    #region private data

    private List<WinLayerDefn> winLayerDefns_ = new List<WinLayerDefn>();
	private WinLayerDefn topLayer_ = null;
	private WinLayerDefn overlaysLayer_ = null;

	#endregion private data


	#region MonoBehaviour

	protected override void PostAwake()
    {
        rectTransform_ = GetComponent<RectTransform>();

        if (numStartingLayers > 0)
        {
            if (DEBUG_LOCAL)
            {
                Debug.Log("WLM: Adding "+numStartingLayers+" starting layers");                       
            }
            for (int i = 0; i < numStartingLayers; i++)
            {
                AddLayer();
            }
        }
		CreateTopLayer( );
//			CreateControlsLayer( );
//		CreateOverlaysLayer( );
    }

    #endregion MonoBehaviour
	#region Setup

    private WinLayerDefn AddLayer()
    {
        GameObject go = Instantiate(winLayerPrefab) as GameObject;
        WinLayerDefn wld = go.GetComponent<WinLayerDefn>();
        wld.Init(winLayersContainer);
        winLayerDefns_.Add(wld);

        if (DEBUG_LOCAL)
        {
            Debug.Log("WLM: Added layer " + NumLayers);
        }
        return wld;
    }

	private WinLayerDefn CreateTopLayer( )
	{
		GameObject go = Instantiate( winLayerPrefab ) as GameObject;
		WinLayerDefn wld = go.GetComponent<WinLayerDefn>( );
		wld.Init( "TopLayer", topLayerContainer );
		topLayer_ = wld;

		if (DEBUG_LOCAL)
		{
			Debug.Log( "WLM: Created top layer ");
		}
		return wld;
	}

	private WinLayerDefn CreateOverlaysLayer( )
	{
		GameObject go = Instantiate( winLayerPrefab ) as GameObject;
		WinLayerDefn wld = go.GetComponent<WinLayerDefn>( );
		wld.Init( "OverlaysLayer", overlaysLayerContainer );
		overlaysLayer_ = wld;
		if (DEBUG_LOCAL)
		{
			Debug.Log( "WLM: Created overlays layer " );
		}
		return wld;
	}

	private WinLayerDefn CreateControlsLayer( )
	{
		GameObject go = Instantiate( winLayerPrefab ) as GameObject;
		WinLayerDefn wld = go.GetComponent<WinLayerDefn>( );
		wld.Init( "ControlsLayer", controlsLayerContainer );
		overlaysLayer_ = wld;
		if (DEBUG_LOCAL)
		{
			Debug.Log( "WLM: Created controls layer " );
		}
		return wld;
	}

	#endregion Setup

	#region Helpers
	private void Add( WinLayerWin win )
	{
		WinLayerDefn layerToPutIn = null;
		for (int i = 0; i < NumLayers && layerToPutIn == null; i++)
		{
			if (winLayerDefns_[i].IsEmpty)
			{
				layerToPutIn = winLayerDefns_[i];
				if (DEBUG_LOCAL)
				{
					Debug.Log( "WLM: Found layer " + i + " of " + NumLayers + " for " + win.gameObject.name );
				}
			}
		}
		if (layerToPutIn == null)
		{
			if (DEBUG_LOCAL)
			{
				Debug.Log( "WLM: No empty layer in " + NumLayers + " for " + win.gameObject.name + ", creating" );
			}
			if (winLayerDefns_.Count > 0)
			{
				WinLayerWin wlw = winLayerDefns_[NumLayers - 1].Content;
				if (wlw != null)
				{
					if (wlw.lossOfFocusAction != null)
					{
						wlw.lossOfFocusAction( );
					}
				}

			}
			layerToPutIn = AddLayer( );
		}
		if (layerToPutIn == null)
		{
			Debug.LogError( "Failed to get layer!" );
		}
		win.AddToWinLayer( layerToPutIn );
		win.MovetoTop( );
	}

	private void AddToTopLayer( WinLayerWin win )
	{
		WinLayerDefn layerToPutIn = topLayer_;
		win.AddToWinLayer( layerToPutIn );
	}

	private void AddToOverlaysLayer( WinLayerWin win )
	{
		WinLayerDefn layerToPutIn = overlaysLayer_;
		win.AddToWinLayer( layerToPutIn );
	}
	#endregion Helpers



}

