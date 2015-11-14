using UnityEngine;
using System.Collections;

public class UIManager : SingletonApplicationLifetimeLazy< UIManager >
{
	private Vector2 screenSize_;
	private Vector2 screenCanvasSize_;

	private readonly float canvasReferenceHeight_ = 600f;

	public Vector2 ScreenSize
	{
		get { return screenSize_; }
	}

	public Vector2 ScreenCanvasSize
	{
		get { return screenCanvasSize_;  }
	}

	protected override void PostAwake()
	{
		screenSize_ = new Vector2( Screen.width, Screen.height );
		screenCanvasSize_ = new Vector2(  canvasReferenceHeight_ * Screen.width / Screen.height, canvasReferenceHeight_ );
		Debug.Log( "UIMan: screen = " + screenSize_ + ", canvas = " + screenCanvasSize_ );
	}

}
