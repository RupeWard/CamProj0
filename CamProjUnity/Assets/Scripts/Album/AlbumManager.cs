﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlbumManager : SingletonSceneLifetime<AlbumManager>
{
	private static readonly bool DEBUG_LOCAL = true;

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

		set
		{
			if (albums_.Contains( value ))
			{

				currentAlbum_ = value;
			}
			else
			{
				Debug.LogError( "ERROR" );
			}
		}
	}

	private bool ioInProgress_ = false;
	public bool IOInProgress
	{
		get { return ioInProgress_; }
	}


	private List<Album> albums_ = new List<Album>( );
	public List<Album> Albums
	{
		get { return albums_;  }
	}

	public System.Action allAlbumsLoadedAction;

	protected override void PostAwake( )
	{
		basePath_ = Application.persistentDataPath;
		allAlbumsLoadedAction = LoadDefaultAlbumCallback;
	}

	public void Start()
	{
		StartCoroutine( loadAlbumsCR( ) );
	}

	public System.Action<Album> currentAlbumChangeActon;

	private void LoadDefaultAlbumCallback()
	{
		allAlbumsLoadedAction -= LoadDefaultAlbumCallback;
		currentAlbum_ = null;
		if (albums_ != null && albums_.Count > 0)
		{
			currentAlbum_ = albums_[0];
			Debug.Log( "Set album to first = " + currentAlbum_.DebugDescribe( ) );
			LogManager.Instance.AddLine( "Set Default Album to first '" + currentAlbum_.AlbumName + "'" );
			if (currentAlbumChangeActon != null)
			{
				currentAlbumChangeActon( currentAlbum_ );
			}
		}
		else
		{
			Debug.Log( "No Albums to choose default from" );
			LogManager.Instance.AddLine( "No Albums to choose Default from" );
		}
	}

	public void AddToCurrentAlbum(AlbumTexture at)
	{
		if (DEBUG_LOCAL)
		{
			Debug.Log( "Added texture " + at.DebugDescribe( ) + " to " + CurrentAlbum.DebugDescribe( ) );
		}
		CurrentAlbum.AddTexture( at );
		LogManager.Instance.AddLine( "Added '"+at.imageName+"' to '" + currentAlbum_.AlbumName + "'" );

		if (DEBUG_LOCAL)
		{
			Debug.Log( "Album following add: " + CurrentAlbum.DebugDescribe( ) );
		}
	}



	#region IO

	private string basePath_;
	private string AlbumsPath
	{
		get { return basePath_ + "/Albums"; }
	}
	private string AlbumPath(Album a)
	{
		return AlbumPath(a.AlbumName);
	}
	private string AlbumPath( string s )
	{
		return AlbumsPath + "/" + s;
	}

	private string TexturePath(Album a, AlbumTexture t)
	{
		return AlbumPath( a )+"/"+t.imageName+".png";
	}

	private string TexturePath( Album a, string t)
	{
		return AlbumPath( a ) + "/" + t+ ".png";
	}

	public bool DeleteAlbumTexture( Album a, AlbumTexture t, System.Action onCompleteAction )
	{
		bool result = false;
		if (ioInProgress_)
		{
			LogManager.Instance.AddLine( "Wait to delete" );
		}
		else
		{
			ioInProgress_ = true;
			string path = TexturePath( a, t );
			if (System.IO.File.Exists( path ))
			{
				System.IO.File.Delete( path );
				LogManager.Instance.AddLine( "Deleted " + path );
				result = true;
			}
			else
			{
				Debug.LogWarning( "Can't delete nonexistent '" + path + "'" );
				LogManager.Instance.AddLine( "Can't delete nonexistent '" + path + "'" );
			}
			if (onCompleteAction != null)
			{
				onCompleteAction( );
			}
			ioInProgress_ = false;
		}
		return result;
	}

    public void SaveAlbumTexture(Album a, AlbumTexture t, System.Action onCompleteAction )
	{
		if (ioInProgress_)
		{
			LogManager.Instance.AddLine( "Wait to save" );
		}
		else
		{
			ioInProgress_ = true;
			string albumPath = AlbumPath( a );
			if (!System.IO.Directory.Exists( AlbumsPath ))
			{
				Debug.Log( "Creating albums path " + AlbumsPath );
				System.IO.Directory.CreateDirectory( AlbumsPath );

			}
			if (!System.IO.Directory.Exists( albumPath ))
			{
				Debug.Log( "Creating album path " + albumPath );
				System.IO.Directory.CreateDirectory( albumPath );
			}
			string texturePath = TexturePath( a, t );
			a.locked = true;
			byte[] bytes = t.texture.EncodeToPNG( );

			System.IO.File.WriteAllBytes( texturePath, bytes );
			t.HandleSaved( );
			a.locked = false;
			if (onCompleteAction != null)
			{
				onCompleteAction( );
			}
			ioInProgress_ = false;
		}
	}

	public void SaveAlbum(Album a, System.Action onCompleteAction)
	{
		if (ioInProgress_)
		{
			LogManager.Instance.AddLine( "Wait to save album" );
		}
		else
		{
			ioInProgress_ = true;
			StartCoroutine( SaveAlbumCR( a, onCompleteAction ) );
		}
	}

	private IEnumerator SaveAlbumCR(Album a, System.Action onCompleteAction )
	{
		a.locked = true;

		System.Text.StringBuilder sb = new System.Text.StringBuilder( );
		sb.Append( "Saving Album " );
		a.DebugDescribe( sb );

		int numSaved = 0;
		int numModified = 0;
		int numUnsaved = 0;

		List<AlbumTexture> textures = a.AlbumTextures;
		foreach (AlbumTexture at in textures)
		{
			bool doSave = true;
			switch( at.IOState)
			{
				case AlbumTexture.EIOState.Saved:
				{
					numSaved++;
					doSave = false;
					sb.Append( "\n ALREADY SAVED: " ).Append( at.imageName );
					break;
				}
				case AlbumTexture.EIOState.Modified:
					{
						numModified++;
						sb.Append( "\n Changes SAVED: " ).Append( at.imageName );
						break;
					}
				case AlbumTexture.EIOState.Unsaved:
					{
						numUnsaved++;
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
		if (numUnsaved == 0 && numModified == 0)
		{
			LogManager.Instance.AddLine( "Nothing to save in Album '" + a.AlbumName );
		}
		else
		{
			LogManager.Instance.AddLine( "Saved Album '" + a.AlbumName +" ( N"+numUnsaved+" M"+numModified+" U"+numUnsaved+")");
		}
		ioInProgress_ = false;
	}

	private int numAlbumsToLoad = 0;

	private IEnumerator loadAlbumsCR()
	{
		if (ioInProgress_)
		{
			Debug.LogError( "IO in progress" );
		}
		else
		{
			ioInProgress_ = true;
			yield return new WaitForSeconds( 5f );
			yield return null;
			albums_ = new List<Album>( );

			if (!System.IO.Directory.Exists( AlbumsPath ))
			{
				Debug.LogWarning( "No Albums folder = no albums" );
			}
			else
			{
				string[] albumDirs = System.IO.Directory.GetDirectories( AlbumsPath );
				if (albumDirs.Length == 0)
				{
					Debug.LogWarning( "No Albums subfolders = no albums" );
				}
				else
				{
					numAlbumsToLoad += albumDirs.Length;
					for (int i = 0; i < albumDirs.Length; i++)
					{
						int index = albumDirs[i].LastIndexOfAny( new char[] { '\\', '/' } );
						string albumDir = albumDirs[i].Substring( index + 1 );
						Debug.Log( "Loading album " + (i + 1) + " of " + albumDirs.Length + " (" + numAlbumsToLoad + ") Name ='" + albumDir + "' '" + albumDirs[i] + "'" );
						yield return StartCoroutine( loadAlbumCR( albumDir, HandleAlbumLoaded ) );
					}
				}
			}
		}
		yield return null;
		ioInProgress_ = false;
	}

	private void HandleNoAlbumsLeftToLoad()
	{
		Debug.Log( "All Albums loaded" );
		if (allAlbumsLoadedAction != null)
		{
			allAlbumsLoadedAction( );
		}
		
	}

	private void HandleAlbumLoaded(Album a)
	{
		if (a != null)
		{
			albums_.Add( a);
			Debug.Log( "AM: Loaded album " + numAlbumsToLoad + ": " + a.DebugDescribe( ) );	
		}
		numAlbumsToLoad--;
		if (numAlbumsToLoad == 0)
		{
			HandleNoAlbumsLeftToLoad( );
		}
	}

	public void LoadAlbum( string albumName, System.Action<Album> onCompleteAction )
	{
		StartCoroutine( loadAlbumCR(albumName, onCompleteAction ));
	}

    private IEnumerator loadAlbumCR( string albumName, System.Action<Album> onCompleteAction )
	{
		Album album = null;

		string albumPath = AlbumPath(albumName);
		album = new Album( albumName );
		if (!System.IO.Directory.Exists(albumPath))
		{
			Debug.LogWarning( "No album '" + albumName + " to load" );
		}
		else
		{
			System.IO.DirectoryInfo albumFolder = new System.IO.DirectoryInfo( albumPath );
			foreach (System.IO.FileInfo file in albumFolder.GetFiles("*.png"))
			{
				string imageName = file.Name.Replace(".png","");
				Debug.Log( "Found file " + imageName );

				string texturePath = TexturePath( album, imageName );

				string dataPath = "file://" +file.FullName;

				Debug.Log( "Attempting to load " + imageName +"( "+file.FullName+" )"+ " from " + dataPath );
				yield return null;
                WWW www = new WWW( dataPath );
				yield return www;
				if (string.IsNullOrEmpty(www.error))
				{
					Texture2D texture = www.texture;
					if (texture == null)
					{
						Debug.LogWarning( "No texture in '" + file.FullName + "'" );
					}
					else
					{
						AlbumTexture at = new AlbumTexture( );
						at.imageName = imageName;
						at.texture = texture;
						album.AddTexture( at );
						at.HandleSaved( );
						Debug.Log( "Added texture " + at.DebugDescribe( ) +" to album");
					}
				}
				else
				{
					Debug.LogError( "Error loading '" + file.FullName + "' with error "+ ((www.error== null)?("null"):(www.error)) );
				}
			}
			Debug.Log( "Loaded album " + album.DebugDescribe( ) );
			LogManager.Instance.AddLine( "Loaded Album '" + album.AlbumName + "' (" + album.NumTextures + " ics)" );
			/*
			for (int i = 0; i < 400; i++)
			{
				LogManager.Instance.AddLine( "Loaded Album '" + album.AlbumName + "' (" + album.NumTextures + " ics)" );
			}
			*/
		}

		if (onCompleteAction != null)
		{
			onCompleteAction( album );
		}
		yield return null;
	}

	#endregion IO

}
