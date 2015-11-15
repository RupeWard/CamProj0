using UnityEngine;
using System.Collections;

abstract public class ImageProcessor_Base : MonoBehaviour
{
	static private readonly bool DEBUG_LOCAL = true;

	public System.Action<ImageProcessor_Base> onSuccessAction;
	public System.Action<ImageProcessor_Base> onFailedAction;

	protected Texture2D texture_;
	public Texture2D Texture
	{
		get { return texture_; }
	}

	public virtual void Init( Texture2D t, System.Action<ImageProcessor_Base> sa, System.Action<ImageProcessor_Base>  fa)
	{
		texture_ = t;
		onSuccessAction = sa;
		onFailedAction = fa;
	}

	protected abstract IEnumerator ProcessImageCR( );

	public void Start()
	{
		if (DEBUG_LOCAL)
		{
			Debug.Log( gameObject.name + " Start" );
		}
		if (onSuccessAction != null && onFailedAction != null)
		{
			if (texture_ != null)
			{
				StartCoroutine( ProcessImageCR( ) );
			}
			else
			{
				Debug.LogError( gameObject.name + " needs texture" );
			}
		}
		else
		{
			Debug.LogError( gameObject.name + " needs callbakcs" );
		}
	}

	protected void HandleFail()
	{
		if (onFailedAction != null)
		{
			onFailedAction( this );
		}
	}

	protected void HandleSuccess( )
	{
		if (onSuccessAction != null)
		{
			onSuccessAction( this );
		}
	}
}
