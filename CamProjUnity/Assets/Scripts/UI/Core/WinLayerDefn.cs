﻿using UnityEngine;
using System.Collections;

 [RequireComponent (typeof(RectTransform))]
 public class WinLayerDefn : MonoBehaviour, IDebugDescribable
 {
	static private readonly bool DEBUG_LOCAL = true;

    #region Interface

    public int LayerNum
    {
		get { return layerNum_; }
	}

	public bool IsEmpty
	{
		get { return currentContent_ == null;  }
	}

	public WinLayerWin Content
	{
		get { return currentContent_;  }
	}

	public void SetContent( WinLayerWin win )
	{
		currentContent_ = win;
	}

	public void ClearContent( )
	{
		currentContent_ = null;
	}

	public WinLayerManager WinLayerManager
	{
		get { return winLayerManager_;  }
	}

	public bool IsOnTop
	{
		get { return (layerNum_ == winLayerManager_.NumLayers - 1);  }
	}

	public bool MoveContentsToTop( )
	{
		return winLayerManager_.MoveContentsToTop( layerNum_ );
	}

	public WinLayerWin ReleaseContents( )
	{
		WinLayerWin wlw = currentContent_;
		ClearContent( );
		return wlw;
	}
    #endregion Interface

    #region cached hooks
    private RectTransform rectTransform_;

    #endregion cached hooks

    #region private data

	private WinLayerManager winLayerManager_;
    private int layerNum_;
	private WinLayerWin currentContent_ = null;

    #endregion private data

    #region MonoBehaviour

    private void Awake()
    {
		rectTransform_ = GetComponent< RectTransform >();
	}

    #endregion MonoBehaviour

    #region Setup

    public void Init(WinLayerManager wlm, RectTransform parent)
    {
		winLayerManager_ = wlm;
		currentContent_ = null;

		layerNum_ = wlm.NumLayers;
        gameObject.name = "Layer_" + layerNum_.ToString("00");
        rectTransform_.SetParent(parent);
		rectTransform_.offsetMin = Vector2.zero;
		rectTransform_.offsetMax = Vector2.zero;
//            rectTransform_.anchoredPosition = Vector2.zero;
        rectTransform_.localScale = Vector3.one;

        if (DEBUG_LOCAL)
        {
			Debug.Log("WLD: Init " + layerNum_ + " " + gameObject.name);
        }
	}

	public void Init( string name, WinLayerManager wlm, RectTransform parent )
	{
		winLayerManager_ = wlm;
		currentContent_ = null;

		layerNum_ = -1;
		gameObject.name = name;
		rectTransform_.SetParent( parent );
		rectTransform_.offsetMin = Vector2.zero;
		rectTransform_.offsetMax = Vector2.zero;
		//            rectTransform_.anchoredPosition = Vector2.zero;
		rectTransform_.localScale = Vector3.one;

		if (DEBUG_LOCAL)
		{
			Debug.Log( "WLD: Init " + gameObject.name );
		}
	}


	#endregion Setup

	#region IDebugDescribable

	public void DebugDescribe( System.Text.StringBuilder sb)
	{
		sb.Append( "[WLD: " ).Append( layerNum_ ).Append( " " ).Append( gameObject.name ).Append(" ");
		if (currentContent_ == null)
		{
			sb.Append( "EMPTY" );
		}
		else
		{
			sb.Append( currentContent_.gameObject.name );
		}
		sb.Append( "]" );
	}
	#endregion IDebugDescribable
	
}

