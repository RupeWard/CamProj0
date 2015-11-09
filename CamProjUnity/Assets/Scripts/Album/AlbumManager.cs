using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlbumManager : SingletonSceneLifetime<AlbumManager>
{
	private static readonly bool DEBUG_LOCAL = true;

	public GameObject albumViewPrefab;

	private Album currentAlbum_ = null;
	public Album CurrentAlbum
	{
		get
		{
			if (currentAlbum_ == null)
			{
				currentAlbum_ = new Album( "DefaultAlbum" );
			}
			return currentAlbum_;
		}
	}

	protected override void PostAwake( )
	{
		basePath_ = Application.persistentDataPath;
	}

	public void AddToCurrentAlbum(AlbumTexture at)
	{
		if (DEBUG_LOCAL)
		{
			Debug.Log( "Added texture " + at.DebugDescribe( ) + " to " + CurrentAlbum.DebugDescribe( ) );
		}
		CurrentAlbum.AddTexture( at );
		if (DEBUG_LOCAL)
		{
			Debug.Log( "Album following add: " + CurrentAlbum.DebugDescribe( ) );
		}
	}

	AlbumViewPanel albumViewPanel_ = null;
	WinLayerWin albumViewWLW_ = null;

	public void ToggleAlbumView()
	{
		if (albumViewPanel_ == null)
		{
			WinLayerWin wlw = WinLayerManager.Instance.InstantiateToLayer( albumViewPrefab );
			if (wlw != null)
			{
				albumViewPanel_ = wlw.transform.GetComponentInChildren<AlbumViewPanel>( );
				if (albumViewPanel_ == null)
				{
					Debug.LogError( "Couldn't find AVP in instantiated prefab" );
				}
				else
				{
					albumViewWLW_ = wlw;
                    albumViewPanel_.Init( CurrentAlbum );
				}
			}
		}
		else
		{
			if (albumViewWLW_ != null)
			{
				WinLayerManager.Instance.RemoveContentsFromLayer( albumViewWLW_ );
				GameObject.Destroy( albumViewWLW_.gameObject );
				albumViewPanel_ = null;
				albumViewWLW_ = null;
			}
			else
			{
				Debug.LogError( "No WLW" );
			}
		}
	}

	private string basePath_;
	private string AlbumsPath
	{
		get { return basePath_ + "/Albums"; }
	}
	private string AlbumPath(Album a)
	{
		return AlbumsPath + "/" + a.AlbumName;
	}
	private string TexturePath(Album a, AlbumTexture t)
	{
		return AlbumPath( a )+"/"+t.imageName+".png";
	}

	public void SaveAlbumTexture(Album a, AlbumTexture t, System.Action onCompleteAction )
	{
		string albumPath = AlbumPath( a );
		if (!System.IO.Directory.Exists( AlbumsPath ))
		{
			Debug.Log( "Creating albums path " + AlbumsPath );
			System.IO.Directory.CreateDirectory( AlbumsPath );

		}
		if (! System.IO.Directory.Exists(albumPath))
		{
			Debug.Log( "Creating album path " + albumPath );
			System.IO.Directory.CreateDirectory( albumPath );
		}
		string texturePath = TexturePath( a, t );
		a.locked = true;
		byte[] bytes = t.texture.EncodeToPNG();

		System.IO.File.WriteAllBytes( texturePath, bytes );
		t.HandleSaved( );
		a.locked = false;
		if (onCompleteAction != null)
		{
			onCompleteAction( );
		}
	}

	public void SaveAlbum(Album a, System.Action onCompleteAction)
	{
		StartCoroutine( SaveAlbumCR( a, onCompleteAction ) );
	}

	private IEnumerator SaveAlbumCR(Album a, System.Action onCompleteAction )
	{
		a.locked = true;

		System.Text.StringBuilder sb = new System.Text.StringBuilder( );
		sb.Append( "Saving Album " );
		a.DebugDescribe( sb );

		List<AlbumTexture> textures = a.AlbumTextures;
		foreach (AlbumTexture at in textures)
		{
			bool doSave = true;
			switch( at.IOState)
			{
				case AlbumTexture.EIOState.Saved:
				{
					doSave = false;
					sb.Append( "\n ALREADY SAVED: " ).Append( at.imageName );
					break;
				}
				case AlbumTexture.EIOState.Modified:
					{
						sb.Append( "\n Changes SAVED: " ).Append( at.imageName );
						break;
					}
				case AlbumTexture.EIOState.Unsaved:
					{
						sb.Append( "\n Newly  SAVED: " ).Append( at.imageName );
						break;
					}

			}
			if (doSave)
			{
				SaveAlbumTexture( a, at, null );
				yield return null;
			}
			a.locked = false;
			if (onCompleteAction != null)
			{
				onCompleteAction( );
			}
		}
	}
}
