using UnityEngine;
using System.Collections;

[RequireComponent ( typeof(RectTransform))]
public abstract class WinWin < TWinType> : MonoBehaviour 
{
	private static readonly bool DEBUG_LOCAL = false;

	public GameObject controlPanelPrefab;
	public Transform overlaysContainer;

	private RectTransform rectTransform_;

	private WinControlPanel< TWinType> controlPanel_ = null;
	Vector2 parentDims;

	public WinLayerWin winLayerWin;

    protected virtual void Awake()
	{
		rectTransform_ = GetComponent<RectTransform>( );
	}

	protected virtual void Start()
	{
		parentDims = UIManager.Instance.ScreenCanvasSize;
		winLayerWin.lossOfFocusAction += HandleLossOfFocus;
		LoadSizeAndPosition( );
	}

	private void OnDestroy()
	{
		Debug.LogWarning( "OnDestroy " + gameObject.name );
		HandleLossOfFocus( );
		if (controlPanel_ != null)
		{
			GameObject.Destroy( controlPanel_.gameObject );
			controlPanel_ = null;
		}
	}

	public void HandleLossOfFocus( )
	{
		if (controlPanel_ != null)
		{
			if (DEBUG_LOCAL)
			{
				Debug.LogWarning( "LOF CP " + gameObject.name );
			}
			controlPanel_.OnDoneButtonPressed( );
		}
		if (isMoving_)
		{
			StopMoving( );
		}
		if (isScaling_)
		{
			StopScaling( );
		}
	}

	private Vector2 lastScreenPosition_;
	private float startingDistFromCentre_;

	private Vector3 startingSize_;
	private Vector3 startingScale_;

	public RectTransform scaleableRT;

	private float CanvasReferenceHeight
	{
		get { return 600f; }
	}

	private float CanvasReferenceWidth
	{
		get { return Screen.width * CanvasReferenceHeight / Screen.height; }
	}


	private Vector2 centreOfWinInScreenCoords_;

    public void HandlePointerDown()
	{
		lastScreenPosition_ = Input.mousePosition;

		centreOfWinInScreenCoords_ =
			new Vector2(
				0.5f * CanvasReferenceWidth + rectTransform_.anchoredPosition.x,
				0.5f * CanvasReferenceHeight + rectTransform_.anchoredPosition.y );

		if (isSizing_)
		{
			if (DEBUG_SIZING)
			{
				Debug.Log( "Centre in screenCoords = " + centreOfWinInScreenCoords_ );
			}
			startingDistFromCentre_ = (lastScreenPosition_ - centreOfWinInScreenCoords_).magnitude;
			startingSize_ = rectTransform_.localScale;
		}
		else if (isScaling_)
		{
			if (DEBUG_SCALING)
			{
				Debug.Log( "Centre in screenCoords = " + centreOfWinInScreenCoords_ );
			}
			if (scaleableRT == null)
			{
				Debug.LogError( "No scaleableRT" );
			}
			startingDistFromCentre_ = (lastScreenPosition_ - centreOfWinInScreenCoords_).magnitude;
			startingScale_ = scaleableRT.localScale;
		}
	}

	private string PPKey
	{
		get
		{
			return "WW_" + gameObject.name+"_";
		}
	}

	public void SaveSizeAndPosition()
	{
		PlayerPrefs.SetFloat( PPKey + "POSX", rectTransform_.anchoredPosition.x );
		PlayerPrefs.SetFloat( PPKey + "POSY", rectTransform_.anchoredPosition.y );
		PlayerPrefs.SetFloat( PPKey + "SIZE", rectTransform_.localScale.x );
		PlayerPrefs.Save( );
		Debug.Log( "Saved size & posn " );
	}

	public void LoadSizeAndPosition( )
	{
		float posx = PlayerPrefs.GetFloat( PPKey + "POSX", -1f );
		float posy = PlayerPrefs.GetFloat( PPKey + "POSY", -1f );
		float size = PlayerPrefs.GetFloat( PPKey + "SIZE", -1f );
		if (posx != -1 && posy != -1 && size != -1)
		{
			Vector2 pos = new Vector2( posx, posy );
            SetSizeAndPosition( pos, size );
			Debug.Log( "Loaded size " + size + " posn " + pos );
		}
		else
		{
			Debug.LogWarning( "No stored size and pos for " + gameObject.name );
		}
	}


	public void SetSizeAndPosition(Vector2 newPos, float size)
	{
		Vector3 newScale = new Vector3( size, size, 1 );
		/*
		if (scaleableRT != null)
		{
			Vector2 relativeScale = new Vector2( newScale.x / rectTransform_.localScale.x, newScale.y / rectTransform_.localScale.y );
			Vector2 newScaleableRTScale = scaleableRT.localScale;
			newScaleableRTScale.x /= relativeScale.x;
			newScaleableRTScale.y /= relativeScale.y;
			scaleableRT.localScale = newScaleableRTScale;
		}
		*/
		rectTransform_.localScale = newScale;

		rectTransform_.anchoredPosition = newPos;
	}

	public void HandlePointerDrag()
	{
		if (isScaling_)
		{
			Vector2 newScreenPosition = Input.mousePosition;
			float currentDistFromCentre_ = (newScreenPosition - centreOfWinInScreenCoords_).magnitude;
			Vector2 diff = newScreenPosition - lastScreenPosition_;
			if (diff.magnitude > 0.1f) //FIXME
			{
				float factor = currentDistFromCentre_ / startingDistFromCentre_;
				Vector3 newScale = startingScale_ * factor;
				bool newScaleOk = true;
				if (newScale.x < 0.1f || newScale.x > 5.0f || newScale.y < 0.1f || newScale.y > 5.0f)
				{
					newScaleOk= false;
					if (DEBUG_SCALING)
					{
						Debug.Log( "Scaling stopped by limits" );
					}
				}
				if (newScaleOk)
				{
					scaleableRT.localScale = newScale;
				}
				Vector2 newSize = new Vector2( newScale.x * rectTransform_.GetWidth( ), newScale.y * rectTransform_.GetHeight( ) );

				lastScreenPosition_ = newScreenPosition;
			}
		}
		else if (isSizing_)
		{
			Vector2 newScreenPosition = Input.mousePosition;
			float currentDistFromCentre_ = (newScreenPosition -centreOfWinInScreenCoords_).magnitude;
			Vector2 diff = newScreenPosition - lastScreenPosition_;
			if (diff.magnitude > 0.1f) //FIXME
			{
				float factor = currentDistFromCentre_ / startingDistFromCentre_;
				Vector3 newScale = startingSize_ * factor;
				Vector2 newSize = new Vector2( newScale.x * rectTransform_.GetWidth( ), newScale.y * rectTransform_.GetHeight( ) );

				bool newSizeOk = true;
				if (newSize.x < 100f || newSize.y < 100f)
				{
					newSizeOk = false;
					if (DEBUG_SCALING)
					{
						Debug.Log( "Sizing stopped by min" );
					}
				}
				if (newSizeOk && factor > 1f)
				{
					Rect newRect = new Rect( 
						rectTransform_.anchoredPosition.x - 0.5f * newSize.x,
						rectTransform_.anchoredPosition.y - 0.5f * newSize.y,
						newSize.x,
						newSize.y
                        );
					if (newRect.xMin < -0.5f * parentDims.x
						|| newRect.xMax > 0.5f * parentDims.x
						|| newRect.yMin < -0.5f *parentDims.y
						|| newRect.yMax > 0.5f *parentDims.y
						)
					{
						newSizeOk = false;
						if (DEBUG_SCALING)
						{
							Debug.Log( "Sizing stopped by edge" );
						}
					}
				}
				if (newSizeOk)
				{
					rectTransform_.localScale = newScale;
				}
				lastScreenPosition_ = newScreenPosition;
			}
		}
		else if (isMoving_)
		{
			Vector2 newScreenPosition = Input.mousePosition;
			Vector2 diff = newScreenPosition - lastScreenPosition_;
			if (diff.magnitude > 0.1f) //FIXME
			{
				Vector2 currentSize = new Vector2( rectTransform_.localScale.x * rectTransform_.GetWidth( ), rectTransform_.localScale.y * rectTransform_.GetHeight( ) );

				{
					float signXMovement = diff.x / Mathf.Abs( diff.x );
					float distFromXEdge = 0f;
					if (signXMovement != 0f)
					{
						if (signXMovement > 0f)
						{
							distFromXEdge = 0.5f * parentDims.x - (0.5f * currentSize.x + rectTransform_.anchoredPosition.x);
						}
						else if (signXMovement < 0f)
						{
							distFromXEdge = (rectTransform_.anchoredPosition.x - 0.5f * currentSize.x) + 0.5f * parentDims.x;
						}
						if (DEBUG_MOVING)
						{
							Debug.Log( "ParentDims = " + parentDims + " diff = " + diff + " pos = " + rectTransform_.anchoredPosition + " currentDims = " + currentSize + " distFromXEdge=" + distFromXEdge + " (" + signXMovement + ")" );
						}
					}
					if (Mathf.Abs( diff.x ) > distFromXEdge)
					{
						diff.x = signXMovement * distFromXEdge;
						if (DEBUG_MOVING)
						{
							Debug.Log( "Clamped x diff to " + diff.x );
						}
					}
				}

				{
					float signYMovement = diff.y / Mathf.Abs( diff.y );
					float distFromYEdge = 0f;
					if (signYMovement != 0f)
					{
						if (signYMovement > 0f)
						{
							distFromYEdge = 0.5f * parentDims.y - (0.5f * currentSize.y + rectTransform_.anchoredPosition.y);
						}
						else if (signYMovement < 0f)
						{
							distFromYEdge = (rectTransform_.anchoredPosition.y - 0.5f * currentSize.y) + 0.5f * parentDims.y;
						}
						if (DEBUG_MOVING)
						{
							Debug.Log( "ParentDims = " + parentDims + " diff = " + diff + " pos = " + rectTransform_.anchoredPosition + " currentDims = " + currentSize + " distFromYEdge=" + distFromYEdge + " (" + signYMovement + ")" );
						}
					}
					if (Mathf.Abs( diff.y ) > distFromYEdge)
					{
						diff.y = signYMovement * distFromYEdge;
						if (DEBUG_MOVING)
						{
							Debug.Log( "Clamped y diff to " + diff.y );
						}
					}
				}

				rectTransform_.anchoredPosition = rectTransform_.anchoredPosition + diff;
				lastScreenPosition_ = newScreenPosition;
			}

		}
	}

	public void HandlePointerUp( )
	{
		if (isScaling_)
		{
			StopScaling( );
		}
		else if (isSizing_)
		{
			StopSizing( );
		}
		else if (isMoving_)
		{
			StopMoving( );
		}
		else
		{
			if (winLayerWin.MovetoTop( ))
			{
				if (DEBUG_LOCAL)
				{
					Debug.Log( "WW: " + gameObject.name + ".HandleClick() moved to top" );
				}
			}
			else
			{
				if (controlPanel_ == null)
				{
					GameObject go = Instantiate( controlPanelPrefab ) as GameObject;
					controlPanel_ = go.GetComponent<WinControlPanel<TWinType>>( );
					if (controlPanel_ == null)
					{
						Debug.LogError( "WW: Failed to make control panel" );
					}
					else
					{
						if (controlPanel_.GetComponent<RectTransform>() ==null)
						{
							Debug.LogError( "NULL RT" );
						}
						WinLayerManager.Instance.SetControls( controlPanel_.GetComponent<RectTransform>( ) );
						controlPanel_.Init( winLayerWin, this );
						if (DEBUG_LOCAL)
						{
							Debug.Log( "WW: HandleClik() opened controls" );
						}
					}
				}
				else
				{
					WinLayerManager.Instance.CloseControls( );
				}
			}
		}
	}

	private static readonly bool DEBUG_MOVING = false;

	private bool isMoving_ = false;
	private RectTransform currentOverlay_;

	private void StopMoving()
	{
		if (currentOverlay_ != null)
		{
			if (DEBUG_LOCAL)
			{
				Debug.Log( "WW: destroying overlay " + currentOverlay_.gameObject.name );
			}
			GameObject.Destroy( currentOverlay_.gameObject );
			currentOverlay_ = null;
		}
		isMoving_ = false;
	}

	public void MoveWindow()
	{
		if (isMoving_)
		{
			StopMoving( );
		}
		else
		{
			if (isScaling_)
			{
				StopScaling( );
			}
			if (isSizing_)
			{
				StopSizing( );
			}
			string prefabName = "Prefabs/UI/Overlays/WinMoveOverlay";
            GameObject go = Resources.Load<GameObject>( prefabName ) as GameObject;
			if (go == null)
			{
				Debug.LogError( "No prefab " + prefabName );
			}
			else
			{
				currentOverlay_ = (Instantiate<GameObject>( go ) as GameObject).GetComponent<RectTransform>( );
				currentOverlay_.SetParent( overlaysContainer );
				currentOverlay_.offsetMin = Vector2.zero;
				currentOverlay_.offsetMax = Vector2.zero;
				currentOverlay_.localScale = Vector3.one;
				isMoving_ = true;
			}
		}
	}

	private static readonly bool DEBUG_SCALING = false;

	public void StopStuff()
	{
		if (isScaling_)
		{
			StopScaling( );
		}
		if (isMoving_)
		{
			StopMoving( );
		}
		if (isSizing_)
		{
			StopSizing( );
		}
	}

	private bool isScaling_ = false;
	private void StopScaling( )
	{
		if (currentOverlay_ != null)
		{
			if (DEBUG_LOCAL)
			{
				Debug.Log( "WW: destroying overlay " + currentOverlay_.gameObject.name );
			}
			GameObject.Destroy( currentOverlay_.gameObject );
			currentOverlay_ = null;
		}
		isScaling_ = false;
	}

	public void ScaleWindow( )
	{
		if (isScaling_)
		{
			StopScaling( );
		}
		else
		{
			if (isMoving_)
			{
				StopMoving( );
			}
			if (isSizing_)
			{
				StopSizing( );
			}
			string prefabName = "Prefabs/UI/Overlays/WinScaleOverlay";
			GameObject go = Resources.Load<GameObject>( prefabName ) as GameObject;
			if (go == null)
			{
				Debug.LogError( "No prefab " + prefabName );
			}
			else
			{
				currentOverlay_ = (Instantiate<GameObject>( go ) as GameObject).GetComponent<RectTransform>( );
				currentOverlay_.SetParent( overlaysContainer );
				currentOverlay_.offsetMin = Vector2.zero;
				currentOverlay_.offsetMax = Vector2.zero;
				currentOverlay_.localScale = Vector3.one;
				WinScaleOverlay wsO = currentOverlay_.GetComponent<WinScaleOverlay>( );
				wsO.Init( rectTransform_ );
				isScaling_ = true;
			}
		}
	}

	private static readonly bool DEBUG_SIZING = true;

	private bool isSizing_= false;

	private void StopSizing( )
	{
		if (currentOverlay_ != null)
		{
			if (DEBUG_LOCAL)
			{
				Debug.Log( "WW: destroying overlay " + currentOverlay_.gameObject.name );
			}
			GameObject.Destroy( currentOverlay_.gameObject );
			currentOverlay_ = null;
		}
		isSizing_ = false;
	}

	public void SizeWindow( )
	{
		if (isSizing_)
		{
			StopSizing( );
		}
		else
		{
			if (isScaling_)
			{
				StopScaling( );
			}
			if (isMoving_)
			{
				StopMoving( );
			}
			string prefabName = "Prefabs/UI/Overlays/WinSizeOverlay";
			GameObject go = Resources.Load<GameObject>( prefabName ) as GameObject;
			if (go == null)
			{
				Debug.LogError( "No prefab " + prefabName );
			}
			else
			{
				currentOverlay_ = (Instantiate<GameObject>( go ) as GameObject).GetComponent<RectTransform>( );
				currentOverlay_.SetParent( overlaysContainer );
				currentOverlay_.offsetMin = Vector2.zero;
				currentOverlay_.offsetMax = Vector2.zero;
				currentOverlay_.localScale = Vector3.one;
				WinSizeOverlay wsO = currentOverlay_.GetComponent<WinSizeOverlay>( );
				wsO.Init( rectTransform_ );
				isSizing_ = true;
			}
		}
	}

}
