using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    [RequireComponent (typeof(RectTransform))]
    public class WinLayerManager : MonoBehaviour
    {
        private static readonly bool DEBUG_LOCAL = true;

        #region Interface

        public int NumLayers
        {
            get { return winLayerDefns_.Count;  }
        }

		public void InstantiateToLayer( GameObject prefab )
		{
			GameObject go = Instantiate( prefab ) as GameObject;
			WinLayerWin win = go.GetComponent<WinLayerWin>( );
			Add( win );
		}

		public void InstantiateToTopLayer( GameObject prefab )
		{
			GameObject go = Instantiate( prefab ) as GameObject;
			WinLayerWin win = go.GetComponent<WinLayerWin>( );
			AddToTopLayer( win );
		}

		public void InstantiateToOverlaysLayer( GameObject prefab )
		{
			GameObject go = Instantiate( prefab ) as GameObject;
			WinLayerWin win = go.GetComponent<WinLayerWin>( );
			AddToOverlaysLayer( win );
		}

		public void SetControls( RectTransform r )
		{
			winControlsLayer.SetControls( r );
		}

	public bool MoveContentsToTop( int layerNum )
	{
		bool bMoved = false;
		if (layerNum != (NumLayers-1))
		{
			if (DEBUG_LOCAL)
			{
				Debug.Log( "WLM: moving contents of layer " + layerNum + " to top " + winLayerDefns_[layerNum].DebugDescribe( ) );
			}

			WinLayerWin newTopContents = winLayerDefns_[layerNum].ReleaseContents( );
			newTopContents.currentLayer = null;
			for (int i = layerNum; i < (NumLayers - 1); i++)
			{
				WinLayerWin content = winLayerDefns_[layerNum + 1].ReleaseContents( );
				if (content != null)
				{
					content.AddToWinLayer( winLayerDefns_[layerNum] );
					if (DEBUG_LOCAL)
					{
						Debug.Log( content.gameObject.name + " moved to layer " + winLayerDefns_[layerNum].DebugDescribe( ) );
					}
				}
				else
				{
					Debug.LogWarning( "WLM: empty layer "+winLayerDefns_[layerNum+1].DebugDescribe() );
				}

			}
			if (newTopContents)
			{
				newTopContents.AddToWinLayer( winLayerDefns_[NumLayers - 1] );
				if (DEBUG_LOCAL)
				{
					Debug.Log( newTopContents.gameObject.name + " moved to top layer " + winLayerDefns_[NumLayers-1].DebugDescribe( ) );
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

		private void Awake()
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
			CreateOverlaysLayer( );
        }

        #endregion MonoBehaviour

        #region Setup

        private WinLayerDefn AddLayer()
        {
            GameObject go = Instantiate(winLayerPrefab) as GameObject;
            WinLayerDefn wld = go.GetComponent<WinLayerDefn>();
            wld.Init(this, winLayersContainer);
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
			wld.Init( "TopLayer", this, topLayerContainer );
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
			wld.Init( "OverlaysLayer", this, overlaysLayerContainer );
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
			wld.Init( "ControlsLayer", this, controlsLayerContainer );
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
				layerToPutIn = AddLayer( );
			}
			if (layerToPutIn == null)
			{
				Debug.LogError( "Failed to get layer!" );
			}
			win.AddToWinLayer( layerToPutIn );
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

