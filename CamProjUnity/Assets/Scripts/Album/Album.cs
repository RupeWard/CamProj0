using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Album: IDebugDescribable
{
	private List<AlbumTexture> textures_ = new List<AlbumTexture>( );

	public void AddTexture( AlbumTexture tex)
	{
		tex.imageName = findNextImagename( tex.imageName );
		textures_.Add( tex );
	}

	private bool imagenameExists(string s)
	{
		bool result = false;
		foreach( AlbumTexture at in textures_ )
		{
			if (at.imageName == s)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private string findNextImagename(string s)
	{
		string result = s;
		bool found = false;
		for (int num = 0; num < 100 && !found; num++)
		{
			string test = s + num.ToString( "00" );
			if (!imagenameExists(test))
			{
				result = test;
				found = true;
				break;
			}
		}
		if (!found)
		{
			Debug.LogWarning( "Failed to find free filename for '" + s + "' in " + this.DebugDescribe( ) );
		}
		return result;
	}

	private string albumName_;

	public Album(string n)
	{
		albumName_ = n;
	}

	private Album( ) { }

	public void DebugDescribe(System.Text.StringBuilder sb)
	{
		sb.Append( "[Album: " ).Append( albumName_ ).Append( " " ).Append( textures_.Count );
		for (int i=0; i<textures_.Count; i++)
		{
			sb.Append( " " ).Append( textures_[i].imageName );
		}
		sb.Append( "]" );
	}
}
