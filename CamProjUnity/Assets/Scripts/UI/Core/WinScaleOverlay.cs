using UnityEngine;
using System.Collections;

public class WinScaleOverlay : MonoBehaviour
{
	private RectTransform scalee_;

	public void Init( RectTransform rt)
	{
		scalee_ = rt;
	}
}
