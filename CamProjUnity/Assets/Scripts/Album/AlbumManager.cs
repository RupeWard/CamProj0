using UnityEngine;
using System.Collections;

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
			Debug.LogWarning( "Already exists: TODO = close" );
		}
	}
}
