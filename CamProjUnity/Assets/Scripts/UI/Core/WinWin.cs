using UnityEngine;
using System.Collections;

[RequireComponent ( typeof(RectTransform))]
public abstract class WinWin < TWinType> : MonoBehaviour 
{
	private static readonly bool DEBUG_LOCAL = true;

	public GameObject controlPanelPrefab;

	public Transform overlaysContainer;

	private RectTransform rectTransform_;

	private WinControlPanel< TWinType> controlPanel_ = null;
	Vector2 parentDims;

	public WinLayerWin winLayerWin;

    protected virtual void Awake()
	{
		rectTransform_ = GetComponent<RectTransform>( );
		parentDims = new Vector2( Screen.width, Screen.height );
		winLayerWin.lossOfFocusAction += HandleLossOfFocus;
	}

	public void HandleLossOfFocus( )
	{
		if (controlPanel_ != null)
		{
			controlPanel_.OnCloseButtonPressed( );
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
					if (scaleableRT != null)
					{
						Vector2 relativeScale = new Vector2(newScale.x / rectTransform_.localScale.x, newScale.y / rectTransform_.localScale.y);
						Vector2 newScaleableRTScale = scaleableRT.localScale;
						newScaleableRTScale.x /= relativeScale.x;
						newScaleableRTScale.y /= relativeScale.y;
						scaleableRT.localScale = newScaleableRTScale;
					}
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
			string prefabName = "Prefabs/UI/WinMoveOverlay";
            GameObject go = Resources.Load<GameObject>( prefabName ) as GameObject;
			if (go == null)
			{
				Debug.LogError( "No prefab " + prefabName );
			}
			else
			{
				currentOverlay_ = (Instantiate<GameObject>( go ) as GameObject).GetComponent<RectTransform>( );
				currentOverlay_.SetParent( overlaysContainer );
				currentOverlay_.offsetMin = 20f * Vector2.one;
				currentOverlay_.offsetMax = -20f * Vector2.one;
				currentOverlay_.localScale = Vector3.one;
				isMoving_ = true;
			}
		}
	}

	private static readonly bool DEBUG_SCALING = false;

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
			string prefabName = "Prefabs/UI/WinScaleOverlay";
			GameObject go = Resources.Load<GameObject>( prefabName ) as GameObject;
			if (go == null)
			{
				Debug.LogError( "No prefab " + prefabName );
			}
			else
			{
				currentOverlay_ = (Instantiate<GameObject>( go ) as GameObject).GetComponent<RectTransform>( );
				currentOverlay_.SetParent( overlaysContainer );
				currentOverlay_.offsetMin = 20f * Vector2.one;
				currentOverlay_.offsetMax = -20f * Vector2.one;
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
			string prefabName = "Prefabs/UI/WinSizeOverlay";
			GameObject go = Resources.Load<GameObject>( prefabName ) as GameObject;
			if (go == null)
			{
				Debug.LogError( "No prefab " + prefabName );
			}
			else
			{
				currentOverlay_ = (Instantiate<GameObject>( go ) as GameObject).GetComponent<RectTransform>( );
				currentOverlay_.SetParent( overlaysContainer );
				currentOverlay_.offsetMin = 20f * Vector2.one;
				currentOverlay_.offsetMax = -20f * Vector2.one;
				currentOverlay_.localScale = Vector3.one;
				WinSizeOverlay wsO = currentOverlay_.GetComponent<WinSizeOverlay>( );
				wsO.Init( rectTransform_ );
				isSizing_ = true;
			}
		}
	}

}
