using UnityEngine;
using System.Collections;

public class WinControlsLayer : MonoBehaviour
{
	static readonly private bool DEBUG_LOCAL = false;

	RectTransform currentControls_ = null;

	public void SetControls( RectTransform c )
	{
		if (currentControls_ == c)
		{
			if (DEBUG_LOCAL)
			{
				Debug.Log( "WCL: same controls " + c.gameObject.name );
			}
		}
		else
		{
			if (currentControls_ != null)
			{
				if (DEBUG_LOCAL)
				{
					Debug.Log("WCL: destroying controls "+currentControls_.gameObject.name );
				}
				GameObject.Destroy( currentControls_.gameObject );
				currentControls_ = null;
			}
			currentControls_ = c;
			currentControls_.SetParent( transform );
			currentControls_.anchoredPosition = Vector2.zero;
			currentControls_.localScale = Vector3.one;
		}
	}

	public void CloseControls()
	{
		if (DEBUG_LOCAL)
		{
			Debug.Log( "WCL close controls" );
		}

		GameObject.Destroy( currentControls_.gameObject );
		currentControls_ = null;
	}

}
