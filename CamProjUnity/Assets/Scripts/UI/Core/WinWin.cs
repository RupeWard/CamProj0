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

	public WinLayerWin winLayerWin;

	protected virtual void Awake()
	{
		rectTransform_ = GetComponent<RectTransform>( );
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
			Vector2 diff = lastScreenPosition - newScreenPosition;
			if (diff.magnitude > 0.1f) //FIXME
			{
				rectTransform_.anchoredPosition = rectTransform_.anchoredPosition - diff;
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

}
