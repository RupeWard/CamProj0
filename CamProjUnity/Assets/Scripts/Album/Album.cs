using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Album: IDebugDescribable
{
	private List<AlbumTexture> albumTextures_ = new List<AlbumTexture>( );
	public List<AlbumTexture> AlbumTextures
	{
		get { return albumTextures_;  }
	}

	public System.Action OnAlbumChanged;
	private void HandleAlbumChanged()
	{
		if (OnAlbumChanged != null)
		{
			OnAlbumChanged( );
		}
	}

	public bool Remove(AlbumTexture at)
	{
		bool result = false;
		if (albumTextures_.Contains( at ))
		{
			albumTextures_.Remove( at );
			result = true;
			HandleAlbumChanged( );
		}
		return result;
	}

	public void AddTexture( AlbumTexture tex)
	{
		tex.imageName = findNextImagename( tex.imageName );
		albumTextures_.Add( tex );
		HandleAlbumChanged( );
	}

	private bool imagenameExists(string s)
	{
		bool result = false;
		foreach( AlbumTexture at in albumTextures_ )
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
	public string AlbumName
	{
		get { return albumName_;  }
	}

	public Album(string n)
	{
		albumName_ = n;
	}

	public int NumTextures
	{
		get { return albumTextures_.Count;  }
	}


	private Album( ) { }

	public void DebugDescribe(System.Text.StringBuilder sb)
	{
		sb.Append( "[Album: " ).Append( albumName_ ).Append( " " ).Append( albumTextures_.Count );
		for (int i=0; i<albumTextures_.Count; i++)
		{
			sb.Append( " " ).Append( albumTextures_[i].imageName );
		}
		sb.Append( "]" );
	}
}
