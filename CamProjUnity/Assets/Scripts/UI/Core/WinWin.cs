using UnityEngine;
using System.Collections;

[RequireComponent ( typeof(RectTransform))]
public class WinWin < TWinType> : MonoBehaviour 
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
	}

	Vector2 lastScreenPosition;

	public void HandlePointerDown()
	{
		if (isMoving_)
		{
			lastScreenPosition = Input.mousePosition;
		}
	}

	public void HandlePointerDrag()
	{
		if (isMoving_)
		{
			Vector2 newScreenPosition = Input.mousePosition;
			Vector2 diff = newScreenPosition - lastScreenPosition;
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
						if (DEBUG_LOCAL)
						{
							Debug.Log( "ParentDims = " + parentDims + " diff = " + diff + " pos = " + rectTransform_.anchoredPosition + " currentDims = " + currentSize + " distFromXEdge=" + distFromXEdge + " (" + signXMovement + ")" );
						}
					}
					if (Mathf.Abs( diff.x ) > distFromXEdge)
					{
						diff.x = signXMovement * distFromXEdge;
						if (DEBUG_LOCAL)
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
						if (DEBUG_LOCAL)
						{
							Debug.Log( "ParentDims = " + parentDims + " diff = " + diff + " pos = " + rectTransform_.anchoredPosition + " currentDims = " + currentSize + " distFromYEdge=" + distFromYEdge + " (" + signYMovement + ")" );
						}
					}
					if (Mathf.Abs( diff.y ) > distFromYEdge)
					{
						diff.y = signYMovement * distFromYEdge;
						if (DEBUG_LOCAL)
						{
							Debug.Log( "Clamped y diff to " + diff.y );
						}
					}
				}

				rectTransform_.anchoredPosition = rectTransform_.anchoredPosition + diff;
				lastScreenPosition = newScreenPosition;
			}

		}
	}

	public void HandlePointerUp( )
	{
		if (isMoving_)
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
						winLayerWin.WinLayerManager.SetControls( controlPanel_.GetComponent<RectTransform>( ) );
						controlPanel_.Init( winLayerWin, this );
						if (DEBUG_LOCAL)
						{
							Debug.Log( "WW: HandleClik() opened controls" );
						}
					}
				}
				else
				{
					winLayerWin.WinLayerManager.CloseControls( );
				}
			}
		}
	}

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

	private bool isScaling_= false;

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
				isMoving_ = true;
			}
		}
	}

}
