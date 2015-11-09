using UnityEngine;
using System.Collections;

[RequireComponent(
	typeof(RectTransform)
)]
public class WinLayerWin : MonoBehaviour
{
	private static readonly bool DEBUG_LOCAL = false;

	public System.Action lossOfFocusAction;

	#region Interface

	public RectTransform RectTransform
	{
		get { return rectTransform_;  }
	}

	public bool MovetoTop( )
	{
		bool bMoved = false;
		if (!currentLayer.IsOnTop)
		{
			bMoved = currentLayer.MoveContentsToTop( );
		}
		return bMoved;
	}

	public bool MoveToBack( )
	{
		if (DEBUG_LOCAL)
		{
			Debug.Log( "WLW: MoveToBack" );
		}

		bool bMoved = false;
		if (!currentLayer.IsAtBack)
		{
			bMoved = currentLayer.MoveContentsToBack( );
		}
		return bMoved;
	}


	public void AddToWinLayer( WinLayerDefn wld )
	{
		if (wld == currentLayer)
		{
			Debug.LogWarning( "WLW: "+gameObject.name+" alrready in layer"+currentLayer.DebugDescribe() );
		}
		else
		{
			if (wld.IsEmpty)
			{
				if (DEBUG_LOCAL)
				{
					Debug.Log( "WLW: " + gameObject.name + "  being added to layer " + wld.DebugDescribe() );
				}
				if (currentLayer != null)
				{
					if (DEBUG_LOCAL)
					{
						Debug.Log( "WLW: " + gameObject.name + "  clearing content of layer " + currentLayer.DebugDescribe( ) );
					}
					currentLayer.ClearContent( );
				}
				if (lossOfFocusAction != null)
				{
					lossOfFocusAction( );
				}
				rectTransform_.SetParent( wld.transform );
				rectTransform_.offsetMin = Vector2.zero;
				rectTransform_.offsetMax = Vector2.zero;
				rectTransform_.localScale = Vector3.one;
				wld.SetContent( this );
				currentLayer = wld;
			}
			else
			{
				Debug.LogError( "WLM: adding win '" + gameObject.name + "' to non-empty layer '" + wld.DebugDescribe( ) );
			}

		}
	}

	#endregion Interface

	#region cached hooks

	private RectTransform rectTransform_;
	
	public WinLayerDefn currentLayer;

	#endregion cached hooks

	void Awake( )
	{
		rectTransform_ = GetComponent<RectTransform>( );
	}



}

