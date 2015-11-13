using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Album: IDebugDescribable
{
	public bool locked = false;

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

	public bool HasUnsavedChanges()
	{
		bool result = false;
		for (int i = 0; !result && i<albumTextures_.Count; i++)
		{
			if (albumTextures_[i] != null && albumTextures_[i].IOState != AlbumTexture.EIOState.Saved)
			{
				result = true;
			}
		}
		return result;
	}
	public bool Remove(AlbumTexture at)
	{
		bool result = false;
		if (!locked)
		{
			if (albumTextures_.Contains( at ))
			{
				albumTextures_.Remove( at );
				result = true;
				HandleAlbumChanged( );
			}
		}
		else
		{
			Debug.LogWarning( "Album is locked, can't remove" );
		}
		return result;
	}

	static System.Text.StringBuilder sb_ = new System.Text.StringBuilder( );

	public string TitleString
	{
		get
		{
			sb_.Length = 0;
			sb_.Append( "'" ).Append( albumName_ ).Append( "' " ).Append( albumTextures_.Count ).Append( " pics" );
			return sb_.ToString( );
		}
	}

	public void AddTexture( AlbumTexture tex)
	{
		if (!locked)
		{
			if (imagenameExists(tex.imageName))
			{
				string nextImageName = findNextImagename( tex.imageName );
				Debug.Log( "Image name " + tex.imageName + " exists so using " + nextImageName);
				tex.imageName = nextImageName;
			}
			albumTextures_.Add( tex );
			HandleAlbumChanged( );
		}
		else
		{
			Debug.LogWarning( "Album is locked, can't add" );
		}
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
