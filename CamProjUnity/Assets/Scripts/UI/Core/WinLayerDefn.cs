using UnityEngine;
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

	public bool IsOnTop
	{
		get { return (layerNum_ == WinLayerManager.Instance.NumLayers - 1);  }
	}

	public bool IsAtBack
	{
		get { return (layerNum_ == 0); }
	}


	public bool MoveContentsToTop( )
	{
		return WinLayerManager.Instance.MoveContentsToTop( layerNum_ );
	}

	public bool MoveContentsToBack( )
	{
		return WinLayerManager.Instance.MoveContentsToBack( layerNum_ );
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

    public void Init(RectTransform parent)
    {
		currentContent_ = null;

		layerNum_ = WinLayerManager.Instance.NumLayers;
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

	public void Init( string name, RectTransform parent )
	{
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

