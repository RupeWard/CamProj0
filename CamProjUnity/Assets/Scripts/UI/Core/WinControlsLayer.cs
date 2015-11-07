using UnityEngine;
using System.Collections;

public class WinControlsLayer : MonoBehaviour
{
	static readonly private bool DEBUG_LOCAL = true;

	RectTransform currentControls_ = null;

	public void SetControls( RectTransform c )
	{
		if (currentControls_ == c)
		{
			Debug.LogWarning( "WCL: same controls " + c.gameObject.name );
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
		}
	}

}
