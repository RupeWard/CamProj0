using UnityEngine;
using System.Collections;

public class GreyScaleImageProcessor : ImageProcessor_Base
{
	static private readonly bool DEBUG_LOCAL = true;

	protected void Awake()
	{
		gameObject.name = "GreyscaleProcessor";
	}

	protected override IEnumerator ProcessImageCR( )
	{
		if (DEBUG_LOCAL)
		{
			Debug.Log( "Starting greyscale process" );
		}
		Texture2D newTexture = new Texture2D( texture_.width, texture_.height );
		if (newTexture == null)
		{
			Debug.LogError( "Failed to create image" );
			LogManager.Instance.AddLine( "Failed to create image" );
			HandleFail( );
		}
		else
		{
			for (int i = 0; i < newTexture.width; i++)
			{
				for (int j = 0; j < newTexture.width; j++)
				{
					Color c = texture_.GetPixel( i, j );
					Color g = new Color( c.grayscale, c.grayscale, c.grayscale );
					newTexture.SetPixel( i, j, g );
				}
				yield return null;
			}
			newTexture.Apply( );
			texture_ = newTexture;
			if (DEBUG_LOCAL)
			{
				Debug.Log( "Finished greyscale process" );
			}
			HandleSuccess( );
		}
	}

}
