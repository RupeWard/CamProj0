using UnityEngine;
using System.Collections;

public class AlbumManager : SingletonApplicationLifetimeLazy<AlbumManager>
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
}
