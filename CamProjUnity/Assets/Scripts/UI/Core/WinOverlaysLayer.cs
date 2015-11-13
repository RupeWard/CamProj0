using UnityEngine;
using System.Collections;

public class WinOverlaysLayer : MonoBehaviour
{
	static readonly private bool DEBUG_LOCAL = false;

	RectTransform currentOverlay_ = null;

	public void SetOverlays( RectTransform c )
	{
		if (currentOverlay_ == c)
		{
			Debug.LogWarning( "WOL: same overlay " + c.gameObject.name );
		}
		else
		{
			if (currentOverlay_ != null)
			{
				if (DEBUG_LOCAL)
				{
					Debug.Log("WOL: destroying overlay "+ currentOverlay_.gameObject.name );
				}
				GameObject.Destroy( currentOverlay_.gameObject );
				currentOverlay_ = null;
			}
			currentOverlay_ = c;
			currentOverlay_.SetParent( transform );
			currentOverlay_.anchoredPosition = Vector2.zero;
			currentOverlay_.localScale = Vector3.one;
		}
	}

	public void CloseOverlays( )
	{
		Debug.LogWarning( "WCL close controls");

		GameObject.Destroy( currentOverlay_.gameObject );
		currentOverlay_ = null;
	}

}
