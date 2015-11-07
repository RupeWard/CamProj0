using UnityEngine;
using System.Collections;

	[RequireComponent (typeof(RectTransform))]
	public class WinLayerWin : MonoBehaviour
	{
		#region Interface

		public WinLayerManager WinLayerManager
		{
			get
			{
				return (currentLayer == null) ?( null):(currentLayer.WinLayerManager);
			}
		}

		public RectTransform RectTransform
		{
			get { return rectTransform_;  }
		}

		public void AddToWinLayer( WinLayerDefn wld )
		{
			if (wld.IsEmpty)
			{
				rectTransform_.SetParent( wld.transform );
				rectTransform_.offsetMin = Vector2.zero;
				rectTransform_.offsetMax = Vector2.zero;
				rectTransform_.localScale = Vector3.one;
				wld.SetContent( this );
				currentLayer = wld;
			}
			else
			{
				Debug.LogError( "WLM: adding win '"+gameObject.name+"' to non-empty layer '"+wld.DebugDescribe() );
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

